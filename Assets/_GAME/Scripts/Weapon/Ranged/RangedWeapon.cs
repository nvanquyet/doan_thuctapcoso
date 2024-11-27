using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ShootingGame.Interface;
namespace ShootingGame
{
    public enum ShootingType
    {
        SingleShot,
        Shotgun,
        Sniper,
        MachineGun,
        BurstFire
    }
    public class RangedWeapon : AWeapon
    {
        [SerializeField] private Projectile _bulletPrefab;
        [SerializeField] private Transform _muzzlePrefab;
        [SerializeField] private Transform[] _bulletSpawnPoint;
        [SerializeField] private Transform _muzzuleSpawnPoint;
        [SerializeField] private ShootingType shootingType = ShootingType.SingleShot;
        [SerializeField] private float burstDelay = 0.1f;
        [SerializeField] private int amountBulletPooling = 10;

        [SerializeField] private int burstCount = 5;

        private ObjectPooling<Projectile> projectilePool;
        private ObjectPooling<Transform> muzzlePool;

        private IDefender defendOwner;

        private List<Projectile> projectileList = new List<Projectile>();
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if(!shootingType.Equals(ShootingType.BurstFire) && !shootingType.Equals(ShootingType.Shotgun)) burstCount = 1;
        }
#endif
        private void Start()
        {
            muzzlePool = new ObjectPooling<Transform>(_muzzlePrefab, amountBulletPooling, transform);
            projectilePool = new ObjectPooling<Projectile>(_bulletPrefab, amountBulletPooling, transform);
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
                switch (shootingType)
                {
                    case ShootingType.SingleShot:
                        ShootSingle();
                        break;
                    case ShootingType.Shotgun:
                        ShootShotgun();
                        break;
                    case ShootingType.Sniper:
                        ShootSniper();
                        break;
                    case ShootingType.MachineGun:
                        StartCoroutine(ShootMachineGun());
                        break;
                    case ShootingType.BurstFire:
                        StartCoroutine(ShootBurstFire(burstCount)); 
                        break;
                }
                return true;
            }
            return false;
        }
        private void ShootSingle()
        {
            FireBullet(_bulletSpawnPoint[0], GetData());
        }
        private (int, bool, int) GetData(bool isPowerful = false)
        {
            var statData = CurrentEquiqmentStat;
            var damage = (int)statData.GetStat(Data.TypeStat.Damage).Value;
            var powerFull = isPowerful ? (UnityEngine.Random.Range(1.5f, 3f)) : 1;
            return ((int)(damage * powerFull), IsCritRate(), (int)statData.GetStat(Data.TypeStat.WeaponForce).Value);
        }
        private void ShootShotgun()
        {
            foreach (var spawnPoint in _bulletSpawnPoint)
            {
                GameService.LogColor($"Shoot");
                FireBullet(spawnPoint, GetData());
            }
        }

        private void ShootSniper()
        {
            FireBullet(_bulletSpawnPoint[0], GetData(true), true); 
        }


        private IEnumerator ShootMachineGun()
        {
            while (true)
            {
                FireBullet(_bulletSpawnPoint[0], GetData());
                yield return new WaitForSeconds(0.2f);  
            }
        }

        private IEnumerator ShootBurstFire(int burstCount)
        {
            for (int i = 0; i < burstCount; i++)
            {
                FireBullet(_bulletSpawnPoint[0], GetData());
                yield return new WaitForSeconds(burstDelay);
            }
        }

        private void FireBullet(Transform spawnPoint, (int, bool, int) data, bool isPowerful = false)
        {
            var bulletClone = projectilePool.Get();
            bulletClone.transform.position = spawnPoint.position;
            bulletClone.OnRecycle = () => RecycleBullet(bulletClone);

            Vector2 direction = (spawnPoint.position - _muzzuleSpawnPoint.position).normalized;
            bulletClone.Spawn(direction, (data.Item1, data.Item2, data.Item3), defendOwner);
            projectileList.Add(bulletClone);
        }


        private void RecycleBullet(Projectile bulletClone)
        {
            if (bulletClone == null) return;
            projectileList.Remove(bulletClone);
            bulletClone.transform.SetParent(transform);
            projectilePool.Recycle(bulletClone);
        }

        private IEnumerator RecycleMuzzle<T>(T target, float time, ObjectPooling<T> pool, Action callback = null) where T : Component
        {
            yield return new WaitForSeconds(time);
            if (target != null) pool.Recycle(target);
            callback?.Invoke();
        }

        private void OnDestroy()
        {
            if(projectileList.Count > 0)
            {
                foreach (var projectile in projectileList)
                {
                    Destroy(projectile.gameObject);
                }
                projectileList.Clear();
            }
        }
        public override int Damage => 0;
    }

}