using System;
using UnityEngine;
using static ShootingGame.Interface;

namespace ShootingGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : AAttacker, Interface.IMoveable, ISpawner
    {
        [SerializeField] protected GameObject hitFX;
        [SerializeField] protected GameObject muzzleFX;
        [SerializeField] private GameObject trailFX;
        [SerializeField] private GameObject projectileFX;

        public Action OnRecycle;
        private bool isCriticalHit;
        private float knockbackForce;
        private Rigidbody2D _rigidbody;

        protected IDefender originatingOwner;
        protected IExpReceiver expReceiver;

        public Rigidbody2D Rigidbody
        {
            get
            {
                if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>();
                return _rigidbody;
            }
        }

        #region Attack
        private static readonly object _lock = new object();
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == null || !CanAttack) return;

            lock (_lock)
            {
                if (collision.TryGetComponent(out Interface.IDefender defender))
                {
                    if (originatingOwner != null && defender.GetType().Equals(originatingOwner.GetType()))
                        return;

                    Attack(defender);

                    if (defender is MonoBehaviour behaviour) ActivateEffect(hitFX, behaviour.transform);
                    else ActivateEffect(hitFX);

                    Rigidbody.velocity = Vector2.zero;

                    _canAttack = false;
                    EnableInteract(false);
                }
            }
        }

        public override bool Attack(Interface.IDefender target, bool isCritical = false, float knockback = 0)
        {
            isCritical = this.isCriticalHit;
            knockback = this.knockbackForce;

            if (base.Attack(target, isCritical, knockback))
            {
                projectileFX?.SetActive(false);
                Invoke(nameof(Recycle), 0.32f);
                return true;
            }
            return false;
        }

        private void Recycle()
        {
            TriggerRecycle();
            DeactivateAllEffects();
        }

        public void TriggerRecycle()
        {
            if (gameObject != null && gameObject.activeInHierarchy) OnRecycle?.Invoke();
        }
        #endregion

        #region Spawn and Move
        public void Move(Vector3 direction) => Rigidbody?.AddForce(direction, ForceMode2D.Impulse);
        public void Move(Vector2 direction) => Rigidbody?.AddForce(direction, ForceMode2D.Impulse);

        public void Spawn() { }

        public void Spawn(Vector2 direction, ImpactData properties,
                IDefender owner = null, IExpReceiver expReceiver = null, float speed = 10f)
        {
            this.originatingOwner = owner;
            this.expReceiver = expReceiver;
            this._canAttack = true;
            this.isCriticalHit = properties.isCritical;
            this.knockbackForce = properties.pushForce;

            Move(direction.normalized * speed);
            SetDamage(properties.damage);

            transform.right = direction;
            transform.SetParent(null);

            hitFX?.gameObject.SetActive(false);

            projectileFX?.gameObject.SetActive(true);
            muzzleFX?.gameObject.SetActive(true);
            trailFX?.gameObject.SetActive(true);

            //Set muzzle direction
            if(muzzleFX) muzzleFX.transform.right = direction;
            muzzleFX.transform.localPosition = Vector3.zero;
            muzzleFX?.transform.SetParent(null);

            Invoke(nameof(Recycle), 2f);
        }
        #endregion


        #region Effect
        protected void ActivateEffect(GameObject effect, Transform hitTs = null, bool isGlobalTransform = true)
        {
            if (effect)
            {
                effect.gameObject.SetActive(true);
                effect.transform.SetParent(isGlobalTransform ? null : transform);
                if (hitTs) effect.transform.position = hitTs.position;
            }
        }

        private void DeactivateAllEffects()
        {
            DeactivateEffect(hitFX);
            DeactivateEffect(muzzleFX);
            DeactivateEffect(projectileFX);
            if(trailFX) DeactivateEffect(trailFX);

            muzzleFX?.transform.SetParent(transform);
            hitFX?.transform.SetParent(transform);
        }

        private void DeactivateEffect(GameObject effect) => effect?.gameObject.SetActive(false);
        #endregion

        #region Exp and Coin
        public override void GainExp(int exp)
        {
            if (expReceiver == null) return;
            expReceiver.GainExp(exp);
        }

        public override void GainCoin(int coin)
        {
            if (expReceiver == null) return;
            expReceiver.GainCoin(coin);
        }
        #endregion
    }
}
