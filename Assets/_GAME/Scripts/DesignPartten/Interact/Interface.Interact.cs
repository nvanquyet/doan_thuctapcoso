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

        public interface IAttackBehaviour
        {
            void ExecuteAttack();
        }
        public interface IExpReceiver
        {
            void GainExp(int exp);

            void GainCoin(int coin);
        }
        public interface IAttacker : IInteract, IExpReceiver
        {
            int Damage { get; }
            bool CanAttack { get; }
            bool Attack(Interface.IDefender target, bool isSuper = false, float forcePushBack = 0);
            void SetDamage(int damage);
            void SetCanAttack(bool value);
        }

        public interface IDefender : IInteract
        {
            int CurrentHealth { get; }
            bool IsDead { get; }
            int ExpGiven { get; } 
            int CoinGiven { get; }
            void Defend(IAttacker attacker, bool isSuper = false, (float, Transform) forceProp = default);
            void Defend(int damage);
            void OnDead(IAttacker attacker);
            void SetHealth(int health, bool resetHealth  = true);
            void PushBack(float force, Transform target);
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

        public virtual void EnableInteract(bool value) => Collider.enabled = value;

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
        protected HashSet<Interface.IInteract> _interactables { get; private set; } = new HashSet<Interface.IInteract>();
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
                ExitInteract(interactable);
            }
        }

        public override void ExitInteract(Interface.IInteract interactable)
        {
            if (_interactables.Contains(interactable)) _interactables.Remove(interactable);
            if (_interactables.Count <= 0) StopCoroutineInteractChecking();
            _canInteract = _interactables.Count < _maxInteractAtSameTime;
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
        protected bool _canAttack = true;
        public virtual int Damage => _damage;
        public bool CanAttack => _canAttack;
        

        public virtual bool Attack(Interface.IDefender target, bool isSuper = false, float forcePushBack = 0)
        {
            if (!CanAttack && Damage <= 0) return false;
            target.Defend(this, isSuper, (forcePushBack, this.transform));
            return true;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null || !CanAttack) return;
            if (other.TryGetComponent(out Interface.IDefender defender))
            {
                Attack(defender);
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

        public abstract void GainExp(int exp);

        public abstract void GainCoin(int coin);
    }

    [RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
    public abstract class ADefender : AInteractable<BoxCollider2D>, Interface.IDefender
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private Rigidbody2D rigid;

        private int _health;
        protected int expGiven;
        protected int coinGiven;
        public Rigidbody2D Rigid {
            get {
                if(!rigid) rigid = GetComponent<Rigidbody2D>();
                return rigid;
            }
        }

        public virtual int MaxHealth => maxHealth;

        public int CurrentHealth => _health;
        public bool IsDead => CurrentHealth <= 0;
        public int ExpGiven => expGiven;
        public int CoinGiven => coinGiven;
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            rigid =  GetComponent<Rigidbody2D>();
        }
#endif

        public virtual void Defend(Interface.IAttacker attacker, bool isSuper = false, (float, Transform) forceProp = default) {
            if (IsDead) return;
            var dmg = (int) (attacker.Damage * (isSuper ? 2 : 1) * UnityEngine.Random.Range(0.8f, 1.1f));
            Defend(dmg);
            this.Dispatch<GameEvent.OnShowFloatingText>(new GameEvent.OnShowFloatingText
            {
                text = $"-{dmg}",
                worldPos = this.transform.position,
                color = isSuper ? Color.red : Color.white
            });
            if (IsDead) OnDead(attacker);
            else if (forceProp != default) PushBack(forceProp.Item1, forceProp.Item2);
        }

        public virtual void OnDead(Interface.IAttacker attacker)
        {
            if (attacker != null)
            {
                attacker.GainExp(ExpGiven);
                attacker.GainCoin(CoinGiven);
            }
        }

        public virtual void SetHealth(int health, bool resetHealth = true) {
            maxHealth = health;
            if(resetHealth)  _health = health;
        }

        public virtual void BuffHealth(int value)
        {
            _health = Mathf.Min(_health + value, maxHealth);
        } 

        public override void ExitInteract(Interface.IInteract target) { }

        public override void OnInteract(Interface.IInteract target) { }

        public void PushBack(float force, Transform target)
        {
            if(Rigid == null) return;
            if (target == null) return;
            var direction = this.transform.position - target.position;
            Rigid.AddForce(direction.normalized * (force * 1.0f / 100), ForceMode2D.Impulse);
        }

        public void Defend(int damage)
        {
            _health -= damage;
        }
    }

}
