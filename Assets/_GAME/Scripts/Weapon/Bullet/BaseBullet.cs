using System;
using UnityEngine;
using static ShootingGame.Interface;

namespace ShootingGame
{
  

    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseBullet : AAttacker, Interface.IMoveable, ISpawner
    {
        [SerializeField] private ParticleSystem trailEffect;
        [SerializeField] private byte speedBullet = 20;

        public Action RecycleAction;
        private bool isSuper;
        private float forcePushBack;
        private Rigidbody2D _rigid;

        public Rigidbody2D Rigid
        {
            get
            {
                if (_rigid == null) _rigid = GetComponent<Rigidbody2D>();
                return _rigid;
            }
        }

        #region  Attack
        public override bool Attack(Interface.IDefender target, bool isSuper = false,  float forcePushBack = 0)
        {
            isSuper = this.isSuper;
            forcePushBack = this.forcePushBack;
            if (base.Attack(target, isSuper, forcePushBack))
            {
                Recycle();
                return true;
            }
            return false;
        }

        private void Recycle()
        {
            if(gameObject.activeInHierarchy) RecycleAction?.Invoke();
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
            Invoke(nameof(Recycle), 2f);
        }
        /// <summary>
        /// Item 1: Damage
        /// Item 2: CritRate
        /// Item 3: Force PushBack
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="bulletProperties"></param>
        public void Spawn(Vector2 direction, (int, bool, float) bulletProperties)
        {
            this.isSuper = bulletProperties.Item2;
            this.forcePushBack = bulletProperties.Item3;
            Move(direction.normalized * speedBullet);
            SetDamage(bulletProperties.Item1);
            if (trailEffect) trailEffect.Play();
            transform.SetParent(null);
            Invoke(nameof(Recycle), 2f);
        }

        public void Spawn(Vector3 direction, bool isCritRate, int damage, bool isSuper = false)
        {
            this.isSuper = isSuper;
            Move(direction.normalized * speedBullet);
            if (trailEffect) trailEffect.Play();
            transform.SetParent(null);
            Invoke(nameof(Recycle), 2f);
        }

        #endregion


    }
}