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

        [SerializeField] private bool _oneHitOnly = true;

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
        public override int Damage => (int) Random.Range(bulletStat.damage * 0.5f, bulletStat.damage * 1.5f);
        public override void SetDamage(int damage) => bulletStat.damage = damage;
        public override bool Attack(Interface.IDefender target)
        {
            if (base.Attack(target) && _oneHitOnly)
            {
                _oneHitOnly = false;
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

        public override void ExitInteract(Interact target) { }

        public override void Interact(Interact target) { }
        #endregion


    }
}