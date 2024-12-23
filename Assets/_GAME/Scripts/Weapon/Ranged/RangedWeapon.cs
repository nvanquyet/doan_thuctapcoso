using ShootingGame.Data;
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
        BurstFire,
        Bazoka
    }
    public class RangedWeapon : AWeapon
    {
        [SerializeField] private Projectile _bulletPrefab;
        [SerializeField] private Transform[] _bulletSpawnPoint;
        [SerializeField] private Transform _muzzuleSpawnPoint;
        [SerializeField] private ShootingType shootingType = ShootingType.SingleShot;
        [SerializeField] private float burstDelay = 0.1f;
        [SerializeField] private int amountBulletPooling = 10;

        [SerializeField] private int burstCount = 5;

        private ObjectPooling<Projectile> projectilePool;

        private IDefender defendOwner;

        private List<Projectile> projectileList = new List<Projectile>();
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (!shootingType.Equals(ShootingType.BurstFire) && !shootingType.Equals(ShootingType.Shotgun)) burstCount = 1;
        }
#endif
        private void Start()
        {
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
                        Invoke(nameof(ShootSniper), attackSpeed * 0.75f);
                        break;
                    case ShootingType.Bazoka:
                        Invoke(nameof(ShootBazoka), attackSpeed * 0.45f);
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

        private ImpactData GetData(bool isPowerful = false)
        {
            var statData = CurrentEquiqmentStat;
            var damage = (int)statData.GetStat(Data.TypeStat.Damage).Value;
            var powerFull = isPowerful ? (UnityEngine.Random.Range(1.5f, 3f)) : 1;
            var isCritRate = IsCritRate();
            return new ImpactData((int)(damage * powerFull), isCritRate, (int)statData.GetStat(Data.TypeStat.WeaponForce).Value);
        }

        private void ShootSingle()
        {
            FireBullet(_bulletSpawnPoint[0], GetData());
        }

        private void ShootShotgun()
        {
            foreach (var spawnPoint in _bulletSpawnPoint)
            {
                FireBullet(spawnPoint, GetData());
            }
        }

        private void ShootSniper()
        {
            FireBullet(_bulletSpawnPoint[0], GetData(true));
        }

        private void ShootBazoka()
        {
            FireBullet(_bulletSpawnPoint[0], GetData());
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

        private void FireBullet(Transform spawnPoint, ImpactData data)
        {
            var bulletClone = projectilePool.Get();
            bulletClone.transform.position = spawnPoint.position;
            bulletClone.OnRecycle = () => RecycleBullet(bulletClone);

            Vector2 direction = (spawnPoint.position - _muzzuleSpawnPoint.position).normalized;
            bulletClone.Spawn(direction, data, defendOwner, expReceiver);
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
            if (projectileList.Count > 0)
            {
                foreach (var projectile in projectileList)
                {
                    Destroy(projectile.gameObject);
                }
                projectileList.Clear();
            }
        }

        public override void GainExp(int exp) { }
        public override void GainCoin(int coin) { }

        public override int Damage => 0;

        public void InitProjectile(int idWeapon)
        {
            var projectileIndex = UserData.GetGunProjectile(idWeapon);
            if (projectileIndex < 0)
            {
                projectilePool = new ObjectPooling<Projectile>(_bulletPrefab, amountBulletPooling, transform);
            }
            else
            {
                var p = GameData.Instance.ProjectileData.GetValue(projectileIndex);
                if (p == null) projectilePool = new ObjectPooling<Projectile>(_bulletPrefab, amountBulletPooling, transform);
                else projectilePool = new ObjectPooling<Projectile>(_bulletPrefab, amountBulletPooling, transform);
            }
        }
    }

}