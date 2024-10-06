using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ShootingGame
{

    public class RangedWeapon : AWeapon
    {
        [SerializeField] private BaseBullet _bulletPrefab;
        [SerializeField] private Transform _muzzlePrefab;
        [SerializeField] private Transform[] _bulletSpawnPoint;
        [SerializeField] private Transform _muzzuleSpawnPoint;
        [SerializeField] private int _amountBulletPooling = 10;

        private ObjectPooling<BaseBullet> _bulletPool;
        private ObjectPooling<Transform> _muzzlePool;
        private Transform _muzzle;

        private void Start()
        {
            _muzzlePool = new ObjectPooling<Transform>(_muzzlePrefab, _amountBulletPooling, transform);
            _bulletPool = new ObjectPooling<BaseBullet>(_bulletPrefab, _amountBulletPooling, transform);
        }

        public override bool Attack()
        {
            if(base.Attack()) {
                var muzzleClone = _muzzlePool.Get();
                muzzleClone.position = _muzzuleSpawnPoint.position;
                muzzleClone.rotation = transform.rotation;
                _muzzle = muzzleClone;
                foreach (Transform spanw in _bulletSpawnPoint)
                {
                    var bulletClone = _bulletPool.Get();
                    bulletClone.transform.position = spanw.position;
                    bulletClone.RecycleAction = RecycleBullet;
                    Vector2 direction = (spanw.position - muzzleClone.position).normalized;
                    bulletClone.Spawn(direction, (int)CurrentEquiqmentStat.Data.GetStat(ShootingGame.Data.TypeStat.Damage).Value);
                    void RecycleBullet()
                    {
                        bulletClone.transform.SetParent(transform);
                        _bulletPool.Recycle(bulletClone);
                    }
                    Invoke(nameof(RecycleBullet), attackSpeed);
                }
                Invoke(nameof(RecycleMuzzle), 0.1f);
                return true;
            }
            return false;
        }
        void RecycleMuzzle() => _muzzlePool.Recycle(_muzzle);


        public override int Damage => 0;
    }

}