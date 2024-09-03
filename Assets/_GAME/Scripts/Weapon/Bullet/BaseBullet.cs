using Unity.VisualScripting;
using UnityEngine;
using static ShootingGame.Interface;

namespace ShootingGame
{
    [System.Serializable]
    public struct BulletStat
    {
        public int minDamage;
        public int maxDamage;
        public int speed;

        public BulletStat(int minDamage, int maxDamage, int speed)
        {
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.speed = speed;
        }
        public int Damage
        {
            get
            {
                return Random.Range(minDamage, maxDamage);
            }
        }
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public class BaseBullet : AAttacker, Interface.IMoveable, ISpawner
    {
        [SerializeField] private BulletStat bulletStat;
        [SerializeField] private ParticleSystem trailEffect;

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
        public override int Damage => bulletStat.Damage;
        public override void SetDamage(int damage) { }
        public override bool Attack(Interface.IDefender target)
        {
            if (base.Attack(target))
            {
                Destroy(gameObject);
                return true;
            }
            return false;
        }

        #endregion

        #region Spawn and Move
        private void OnEnable() => Spawn();
        public void Move(Vector3 direction) => Rigid?.AddForce(direction, ForceMode2D.Impulse);
        public void Spawn()
        {
            Move(transform.right * bulletStat.speed);
            if (trailEffect) trailEffect.Play();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Health>().TakeDam((int)bulletStat.Damage);
                collision.GetComponent<EnemyController>().TakeDamEffect((int)bulletStat.Damage);
                Destroy(gameObject);
            }
        }
        #endregion


    }
}