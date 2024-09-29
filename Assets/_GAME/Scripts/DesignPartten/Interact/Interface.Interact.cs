using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ShootingGame
{
    public partial interface Interface
    {
        public interface IInteract
        {
            Collider2D Collider { get; }

            void OnInteract(Interface.IInteract target);
            void ExitInteract(Interface.IInteract target);
        }

        public interface IAttacker : IInteract
        {
            int Damage { get; }
            bool CanAttack { get; }
            bool Attack(Interface.IDefender target);
            void SetDamage(int damage);
            void SetCanAttack(bool value);
        }

        public interface IDefender : IInteract
        {
            int CurrentHealth { get; }
            bool IsDead { get; }
            void Defend(int damage);
            void OnDead();
            void SetHealth(int health, bool resetHealth  = true);
        }

    }

    public abstract class AInteractable<T> : MonoBehaviour, Interface.IInteract where T : Collider2D
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

        public abstract void OnInteract(Interface.IInteract target);

        public virtual void EnableInteract(bool value) => _collider.enabled = value;

        public abstract void ExitInteract(Interface.IInteract target);

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            
        }
#endif
    }

    [RequireComponent(typeof(BoxCollider2D))]
    public abstract class AInteractor : AInteractable<BoxCollider2D>
    {
        public override void OnInteract(Interface.IInteract target)
        {
            target.OnInteract(this);
        }

        public override void ExitInteract(Interface.IInteract target)
        {
            target.ExitInteract(this);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null) return;
            if (other.TryGetComponent(out Interface.IInteract interactable))
            {
                OnInteract(interactable);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other == null) return;
            if (other.TryGetComponent(out Interface.IInteract interactable))
            {
                ExitInteract(interactable);
            }
        }
    }

    public abstract class AStayInteractor<T> : AInteractable<T> where T : Collider2D
    {
        private HashSet<Interface.IInteract> _interactables = new HashSet<Interface.IInteract>();
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
                    OnInteract(interactable);
                }
                yield return null;
            }
        }

        public override void OnInteract(Interface.IInteract target)
        {
            target.OnInteract(this);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null || !_canInteract) return;
            if (other.TryGetComponent(out Interface.IInteract interactable)) _canInteract = InteractChecking(interactable);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == null || !_canInteract) return;
            if (other.TryGetComponent(out Interface.IInteract interactable))
            {
                if (_interactables.Contains(interactable)) _interactables.Remove(interactable);
                if (_interactables.Count <= 0) StopCoroutineInteractChecking();
                _canInteract = _interactables.Count < _maxInteractAtSameTime;
            }
        }

        void OnTriggerStay2D(Collider2D other)
        {
            if (other == null || !_canInteract) return;
            if (other.TryGetComponent(out Interface.IInteract interactable)) _canInteract = InteractChecking(interactable);
        }



        private bool InteractChecking(Interface.IInteract target)
        {
            if (_interactables == null) _interactables = new HashSet<Interface.IInteract>();
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

        public override void OnInteract(Interface.IInteract target) { }
        public override void ExitInteract(Interface.IInteract target) { }
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

        public override void ExitInteract(Interface.IInteract target) { }

        public override void OnInteract(Interface.IInteract target) { }
    }

}
