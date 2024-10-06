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
        public override bool Attack(Interface.IDefender target)
        {
            if (base.Attack(target))
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
            transform.rotation = Quaternion.identity;
            Move(transform.right * speedBullet);
            if (trailEffect) trailEffect.Play();
            transform.SetParent(null);
            Invoke(nameof(Recycle), 2f);
        }

        public void Spawn(Vector2 direction, int damage)
        {
            Move(direction.normalized * speedBullet);
            SetDamage(damage);
            if (trailEffect) trailEffect.Play();
            transform.SetParent(null);
            Invoke(nameof(Recycle), 2f);
        }

        public void Spawn(Vector3 direction, bool isCritRate, int damage)
        {
            Move(direction.normalized * speedBullet);
            if (trailEffect) trailEffect.Play();
            transform.SetParent(null);
            Invoke(nameof(Recycle), 2f);
        }

        #endregion


    }
}