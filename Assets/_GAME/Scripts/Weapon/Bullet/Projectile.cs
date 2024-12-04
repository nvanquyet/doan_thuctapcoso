using System;
using UnityEngine;
using static ShootingGame.Interface;

namespace ShootingGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : AAttacker, Interface.IMoveable, ISpawner
    {
        [SerializeField] protected ParticleSystem impactEffect;
        [SerializeField] private ParticleSystem projectileTrail;
        [SerializeField] private GameObject visualProjectile;
        [SerializeField] private byte projectileSpeed = 20;

        public Action OnRecycle;
        private bool isCriticalHit;
        private float knockbackForce;
        private Rigidbody2D _rigidbody;
        protected IDefender originatingOwner;

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

                    ActivateEffect(impactEffect);

                    Rigidbody.velocity = Vector2.zero;

                    if (_oneHitOnly)
                    {
                        _canAttack = false; 
                        EnableInteract(false);
                    }
                }
            }
        }

        public override bool Attack(Interface.IDefender target, bool isCritical = false, float knockback = 0)
        {
            isCritical = this.isCriticalHit;
            knockback = this.knockbackForce;

            if (base.Attack(target, isCritical, knockback))
            {
                visualProjectile?.SetActive(false);
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

        public void Spawn()
        {
            isCriticalHit = false;
            _canAttack = true; 
            visualProjectile.gameObject.SetActive(true);
            transform.rotation = Quaternion.identity;
            Move(transform.right * projectileSpeed);
            if (projectileTrail) projectileTrail.Play();
            transform.SetParent(null);
            Invoke(nameof(Recycle), 2f);
        }

        public void Spawn(Vector2 direction, ImpactData properties, IDefender owner = null)
        {
            originatingOwner = owner;
            _canAttack = true;
            isCriticalHit = properties.isCritical;
            knockbackForce = properties.pushForce;
            visualProjectile.gameObject.SetActive(true);
            Move(direction.normalized * projectileSpeed);
            SetDamage(properties.damage);
            if (projectileTrail) projectileTrail.Play();
            transform.right = direction;
            transform.SetParent(null);
            Invoke(nameof(Recycle), 2f);
        }
        #endregion

        protected void ActivateEffect(ParticleSystem effect)
        {
            DeactivateAllEffects();
            if (effect) effect.gameObject.SetActive(true);
        }

        private void DeactivateAllEffects()
        {
            if (impactEffect) DeactivateEffect(impactEffect);
            if (projectileTrail) DeactivateEffect(projectileTrail);
        }

        private void DeactivateEffect(ParticleSystem effect) => effect?.gameObject.SetActive(false);
    }
}
