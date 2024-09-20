using System;
using UnityEngine;
using static ShootingGame.Interface;

namespace ShootingGame
{
    [System.Serializable]
    public struct AttackStat
    {
        public int damage;
        public int speed;

        public AttackStat(int damage, int speed)
        {
            this.damage = damage;
            this.speed = speed;
        }
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseBullet : AAttacker, Interface.IMoveable, ISpawner
    {
        [SerializeField] private AttackStat bulletStat;
        [SerializeField] private ParticleSystem trailEffect;

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
        public override int Damage => (int) UnityEngine.Random.Range(bulletStat.damage * 0.5f, bulletStat.damage * 1.5f);
        public override void SetDamage(int damage) => bulletStat.damage = damage;
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
        public void Spawn()
        {
            transform.rotation = Quaternion.identity;
            Move(transform.right * bulletStat.speed);
            if (trailEffect) trailEffect.Play();
            transform.SetParent(null);
            Invoke(nameof(Recycle), 2f);
        }

        public void Spawn(Vector3 direction)
        {
            Move(direction.normalized * bulletStat.speed);
            if (trailEffect) trailEffect.Play();
            transform.SetParent(null);
            Invoke(nameof(Recycle), 2f);
        }

        #endregion


    }
}