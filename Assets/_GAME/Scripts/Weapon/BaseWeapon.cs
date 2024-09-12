using UnityEngine;
using static ShootingGame.Interface;
namespace ShootingGame
{

    public class BaseWeapon : AAttacker
    {
        [SerializeField] private BaseBullet _bulletPrefab;
        [SerializeField] private Transform _muzzlePrefab;
        [SerializeField] private Transform[] _bulletSpawnPoint;
        [SerializeField] private int _amountBulletPooling = 10;

        [SerializeField] private float _fireRate = 0.45f;

        private ObjectPooling<BaseBullet> _bulletPool;
        private ObjectPooling<Transform> _muzzlePool;

        private float mFireRate = 0;

        private void Start()
        {
            _bulletPool = new ObjectPooling<BaseBullet>(_bulletPrefab, _amountBulletPooling, transform);
            _muzzlePool = new ObjectPooling<Transform>(_muzzlePrefab, _amountBulletPooling, transform);
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

                var bulletClone = _bulletPool.Get();
                bulletClone.transform.position = spanw.position;
                bulletClone.transform.rotation = Quaternion.identity;
                bulletClone.RecycleAction = Recycle;

                Invoke(nameof(Recycle), 1f);
                void Recycle()
                {
                    _bulletPool.Recycle(bulletClone);
                    bulletClone.transform.SetParent(transform);
                    _muzzlePool.Recycle(muzzleClone);
                }
            }
        }

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
    }

}