using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ShootingGame
{
    public partial interface Interface
    {
        public interface Interact
        {
            Collider2D Collider { get; }

            void Interact(Interface.Interact target);
            void ExitInteract(Interface.Interact target);
        }

        public interface IAttacker : Interact
        {
            int Damage { get; }
            bool CanAttack { get; }
            bool Attack(Interface.IDefender target);
            void SetDamage(int damage);
            void SetCanAttack(bool value);
        }

        public interface IDefender : Interact
        {
            int CurrentHealth { get; }
            bool IsDead { get; }
            void Defend(int damage);
            void OnDead();
            void SetHealth(int health, bool resetHealth  = true);
        }

    }

    public abstract class AInteractable<T> : MonoBehaviour, Interface.Interact where T : Collider2D
    {
        protected Collider2D _collider;

        // [SerializeField] bool _canInteract;


        public virtual Collider2D Collider
        {
            get
            {
                if (!_collider) _collider = GetComponent<T>();
                return _collider;
            }
        }
        //public bool CanInteract => _canInteract;

        public abstract void Interact(Interface.Interact target);

        public virtual void EnableInteract(bool value) => _collider.enabled = value;

        public abstract void ExitInteract(Interface.Interact target);

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            
        }
#endif
    }

    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class AInteractor : AInteractable<BoxCollider2D>
    {
        public override void Interact(Interface.Interact target)
        {
            target.Interact(this);
        }

        public override void ExitInteract(Interface.Interact target)
        {
            target.ExitInteract(this);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null) return;
            if (other.TryGetComponent(out Interface.Interact interactable))
            {
                Interact(interactable);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other == null) return;
            if (other.TryGetComponent(out Interface.Interact interactable))
            {
                ExitInteract(interactable);
            }
        }
    }

    public abstract class AStayInteractor<T> : AInteractable<T> where T : Collider2D
    {
        private HashSet<Interface.Interact> _interactables = new HashSet<Interface.Interact>();
        [SerializeField] private int _maxInteractAtSameTime = 20;
        private bool _canInteract = true;

        private Coroutine _coroutineCheckInteract;

        private void StartCouroutineInteractChecking()
        {
            if (_coroutineCheckInteract != null) StopCoroutine(_coroutineCheckInteract);
            _coroutineCheckInteract = StartCoroutine(IECheckInteract());
        }

        private void StopCoroutineInteractChecking()
        {
            if (_coroutineCheckInteract != null) StopCoroutine(_coroutineCheckInteract);
            _coroutineCheckInteract = null;
        }

        private IEnumerator IECheckInteract()
        {
            while (true)
            {
                foreach (var interactable in _interactables)
                {
                    Interact(interactable);
                }
                yield return null;
            }
        }

        public override void Interact(Interface.Interact target)
        {
            target.Interact(this);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null || !_canInteract) return;
            if (other.TryGetComponent(out Interface.Interact interactable)) _canInteract = InteractChecking(interactable);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == null || !_canInteract) return;
            if (other.TryGetComponent(out Interface.Interact interactable))
            {
                if (_interactables.Contains(interactable)) _interactables.Remove(interactable);
                if (_interactables.Count <= 0) StopCoroutineInteractChecking();
                _canInteract = _interactables.Count < _maxInteractAtSameTime;
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (other == null || !_canInteract) return;
            if (other.TryGetComponent(out Interface.Interact interactable)) _canInteract = InteractChecking(interactable);
        }



        private bool InteractChecking(Interface.Interact target)
        {
            if (_interactables == null) _interactables = new HashSet<Interface.Interact>();
            if (_interactables.Count >= _maxInteractAtSameTime) return false;
            if (!_interactables.Contains(target)) _interactables.Add(target);

            if (_interactables.Count == 1) StartCouroutineInteractChecking();
            return true;
        }
    }

    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class AAttacker : AInteractable<BoxCollider2D>, Interface.IAttacker
    {
        [SerializeField] protected int _damage;
        [SerializeField] private bool _oneHitOnly = true;
        protected bool _canAttack = true;
        public virtual int Damage => _damage;
        public bool CanAttack => _canAttack;
        

        public virtual bool Attack(Interface.IDefender target)
        {
            if (!CanAttack && Damage <= 0) return false;
            target.Defend(Damage);
            return true;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null) return;
            if (other.TryGetComponent(out Interface.IDefender defender))
            {
                Attack(defender);
                if(_oneHitOnly) _canAttack = false;
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other == null) return;
            if (other.TryGetComponent(out Interface.IDefender defender))
            {
                ExitInteract(defender);
            }
        }

        public virtual void SetDamage(int damage) => _damage = damage;

        public virtual void SetCanAttack(bool value) => _canAttack = value;

        public override void Interact(Interface.Interact target) { }
        public override void ExitInteract(Interface.Interact target) { }
    }

    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class ADefender : AInteractable<BoxCollider2D>, Interface.IDefender
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int _health;

        public virtual int MaxHealth => maxHealth;

        public int CurrentHealth => _health;
        public bool IsDead => CurrentHealth <= 0;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            _health = maxHealth;
        }
#endif

        public virtual void Defend(int damage) {
            if (IsDead) return;
            _health -= damage;
            if (IsDead) OnDead();
        }
        public abstract void OnDead();

        public void SetHealth(int health, bool resetHealth = true) {
            maxHealth = health;
            if(resetHealth)  _health = health;
        }

        public override void ExitInteract(Interface.Interact target) { }

        public override void Interact(Interface.Interact target) { }
    }

}
