using System;
using UnityEngine;
using static ShootingGame.Interface;

namespace ShootingGame
{


    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseBullet : AAttacker, Interface.IMoveable, ISpawner
    {
        [SerializeField] private ParticleSystem hitEffect;
        [SerializeField] private ParticleSystem bulletEffect;
        [SerializeField] private ParticleSystem trailEffect;
        [SerializeField] private byte speedBullet = 20;

        public Action RecycleAction;
        private bool isSuper;
        private float forcePushBack;
        private Rigidbody2D _rigid;
        private IDefender owner;


        public Rigidbody2D Rigid
        {
            get
            {
                if (_rigid == null) _rigid = GetComponent<Rigidbody2D>();
                return _rigid;
            }
        }

        #region  Attack
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null) return;
            if (other.TryGetComponent(out Interface.IDefender defender))
            {
                if (owner != null && defender == owner)
                {
                    return;
                }
                Attack(defender);
                ActiveEffect(hitEffect);
                Rigid.velocity = Vector2.zero;
                if (_oneHitOnly)
                {
                    _canAttack = false;
                    EnableInteract(false);
                }
            }
        }
        public override bool Attack(Interface.IDefender target, bool isSuper = false, float forcePushBack = 0)
        {
            isSuper = this.isSuper;
            forcePushBack = this.forcePushBack;
            if (base.Attack(target, isSuper, forcePushBack))
            {
                Invoke(nameof(Recycle), 0.1f);
                return true;
            }
            return false;
        }

        private void Recycle()
        {
            InvokeRecycleAction();
            DisactiveAllEffect();
        }


        public void InvokeRecycleAction()
        {
            if(gameObject != null && gameObject.activeInHierarchy) RecycleAction?.Invoke();
        }

        #endregion

        #region Spawn and Move
        public void Move(Vector3 direction) => Rigid?.AddForce(direction, ForceMode2D.Impulse);
        public void Move(Vector2 direction) => Rigid?.AddForce(direction, ForceMode2D.Impulse);
        public void Spawn()
        {
            this.isSuper = false;
            transform.rotation = Quaternion.identity;
            Move(transform.right * speedBullet);
            if (trailEffect) trailEffect.Play();
            transform.SetParent(null);
            ActiveBulletEffect(transform.right.normalized);
            Invoke(nameof(Recycle), 2f);
        }
        /// <summary>
        /// Item 1: Damage
        /// Item 2: CritRate
        /// Item 3: Force PushBack
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="bulletProperties"></param>
        public void Spawn(Vector2 direction, (int, bool, float) bulletProperties, IDefender owner = null)
        {
            this.owner = owner;
            this.isSuper = bulletProperties.Item2;
            this.forcePushBack = bulletProperties.Item3;
            Move(direction.normalized * speedBullet);
            SetDamage(bulletProperties.Item1);
            if (trailEffect) trailEffect.Play();
            transform.SetParent(null);
            ActiveBulletEffect(direction.normalized);
            Invoke(nameof(Recycle), 2f);
        }
        #endregion

        private void ActiveBulletEffect(Vector3 direction)
        {
            ActiveEffect(bulletEffect);
            if (bulletEffect) bulletEffect.transform.right = direction;
        }

        private void ActiveEffect(ParticleSystem effect)
        {
            DisactiveAllEffect();
            if (!effect) return;
            effect.gameObject.SetActive(true);
        }
        private void DisactiveAllEffect()
        {
            if (hitEffect) DisactiveEffect(hitEffect);
            if (bulletEffect) DisactiveEffect(bulletEffect);
            if (trailEffect) DisactiveEffect(trailEffect);
        }
        private void DisactiveEffect(ParticleSystem effect) => effect?.gameObject.SetActive(false);



    }
}