using System;
using UnityEngine;
using VawnWuyest.Data;
using static ShootingGame.Interface;
namespace ShootingGame
{

    public class BaseWeapon : AAttacker
    {
        [SerializeField] private BaseBullet _bulletPrefab;
        [SerializeField] private Transform _muzzlePrefab;
        [SerializeField] private Transform[] _bulletSpawnPoint;
        [SerializeField] private int _amountBulletPooling = 10;

        [SerializeField] private float _fireRate = 0.5f;

        private ObjectPooling<BaseBullet> _bulletPool;
        private ObjectPooling<Transform> _muzzlePool;

        private float mFireRate = 0;
        private BaseBullet _bullet;
        private Transform _muzzle;

        private void Start()
        {
            _muzzlePool = new ObjectPooling<Transform>(_muzzlePrefab, _amountBulletPooling, transform);
            _bulletPool = new ObjectPooling<BaseBullet>(_bulletPrefab, _amountBulletPooling, transform);
        }

        public void Attack()
        {
            if (mFireRate < _fireRate)
            {
                mFireRate += Time.deltaTime;
                return;
            }
            mFireRate = 0;
            foreach (Transform spanw in _bulletSpawnPoint)
            {
                var muzzleClone = _muzzlePool.Get();
                muzzleClone.position = spanw.position;
                muzzleClone.rotation = transform.rotation;
                _muzzle = muzzleClone;
                var bulletClone = _bulletPool.Get();
                bulletClone.transform.position = spanw.position;
                bulletClone.RecycleAction = RecycleBullet;
                bulletClone.Spawn(spanw.right);
                _bullet = bulletClone;
                Invoke(nameof(RecycleBullet), _fireRate);
                Invoke(nameof(RecycleMuzzle), 0.1f);
                
                
            }
        }
        void RecycleBullet()
        {
            _bulletPool.Recycle(_bullet);
            _bullet.transform.SetParent(transform);
        }
        void RecycleMuzzle() => _muzzlePool.Recycle(_muzzle);
        public void Rotate(Vector3 pos)
        {
            Vector2 lookDir = pos - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rotation;

            if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270) transform.localScale = new Vector3(1, -1, 0);
            else transform.localScale = new Vector3(1, 1, 0);
        }

        public override void ExitInteract(Interact target) { }

        public override void Interact(Interact target) { }

        public override int Damage => 0;




        #region Stat
        [SerializeField] private ItemStat equiqment;
        private BaseStat totalStat;

        internal void ApplyStat(IStats stat)
        {
            var baseData = equiqment.Data;
            var allStat = baseData.AllStats;

            totalStat = new BaseStat(baseData.AllStats, true);

            foreach (var s in allStat)
            {
                var target = stat.Data.GetStat(s.TypeStat);
                var baseValue = baseData.GetStat(s.TypeStat).Value;

                var totalValue = totalStat.GetStat(s.TypeStat);
                switch(s.TypeValueStat)
                {
                    case TypeValueStat.FixedValue:
                        totalValue.SetValue(baseValue + target.GetValue(baseValue));
                        break;
                    case TypeValueStat.Percentage:
                        switch(target.TypeValueStat)
                        {
                            case TypeValueStat.FixedValue:
                                totalValue.SetValue(target.Value + s.GetValue(target.Value));
                                break;
                            case TypeValueStat.Percentage:
                                totalValue.SetValue(baseValue + target.Value);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }

}