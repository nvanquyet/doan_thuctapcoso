using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShootingGame.Interface;
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

        private IDefender defendOwner;
        private void Start()
        {
            _muzzlePool = new ObjectPooling<Transform>(_muzzlePrefab, _amountBulletPooling, transform);
            _bulletPool = new ObjectPooling<BaseBullet>(_bulletPrefab, _amountBulletPooling, transform);
            defendOwner = GetComponentInParent<IDefender>();
        }
        public override bool Attack(Interface.IDefender target, bool isSuper = false, float forcePushBack = 0)
        {
            return false;
        }
        public override bool Attack()
        {
            if (base.Attack())
            {
                var muzzleClone = _muzzlePool.Get();
                muzzleClone.position = _muzzuleSpawnPoint.position;
                muzzleClone.rotation = transform.rotation;
                foreach (Transform spanw in _bulletSpawnPoint)
                {
                    var bulletClone = _bulletPool.Get();
                    while (bulletClone.transform.parent == null)
                    {
                        bulletClone = _bulletPool.Get();
                    }
                    GameService.LogColor($"Shoot {bulletClone != null} Parent: {bulletClone.transform.parent}");
                    bulletClone.transform.position = spanw.position;
                    bulletClone.RecycleAction = () => {
                        bulletClone.transform.SetParent(transform);
                        _bulletPool.Recycle(bulletClone);
                    };
                    Vector2 direction = (spanw.position - muzzleClone.position).normalized;
                    var statData = CurrentEquiqmentStat;
                    bulletClone.Spawn(direction, ((int)statData.GetStat(Data.TypeStat.Damage).Value, IsCritRate(),
                                                     statData.GetStat(Data.TypeStat.WeaponForce).GetValue()), defendOwner);
                    StartCoroutine(RecycleMuzzle(bulletClone, attackSpeed, _bulletPool, () =>
                    {
                        bulletClone.transform.SetParent(transform);
                    }));
                }
                StartCoroutine(RecycleMuzzle(muzzleClone, attackSpeed / 10, _muzzlePool));
                return true;
            }
            return false;
        }

        private IEnumerator RecycleMuzzle<T>(T target, float time, ObjectPooling<T> pool, Action callback = null) where T : Component
        {
            yield return new WaitForSeconds(time);
            if (target != null) pool.Recycle(target);
            callback?.Invoke();
        }


        public override int Damage => 0;
    }

}