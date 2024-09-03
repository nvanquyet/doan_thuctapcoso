using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ShootingGame
{
    public class WeaponCtrl : MonoBehaviour
    {
        [SerializeField] private List<Transform> _allPositionSpawnWeapon;
        [SerializeField] private List<BaseWeapon> _weapons;

        public List<Transform> Enemies = new List<Transform>();

        private int MaxWeapon => _allPositionSpawnWeapon.Count;

        private void OnValidate()
        {
            _allPositionSpawnWeapon = GetComponentsInChildren<Transform>().ToList();
            _allPositionSpawnWeapon.RemoveAt(0);
        }

        private void Update()
        {
            Rotate();
            Shoot();
        }

        public void Shoot()
        {
            if (_weapons == null || _weapons.Count <= 0) return;

            foreach (var weapon in _weapons)
            {
                weapon.Attack();
            }
        }

        public void AddWeapon(BaseWeapon newWeapon)
        {
            if (newWeapon == null) return;
            if (_weapons.Count < MaxWeapon && !_weapons.Contains(newWeapon))
            {
                _weapons.Add(newWeapon);
                newWeapon.transform.SetParent(_allPositionSpawnWeapon[_weapons.Count]);
            }
        }

        public void Rotate()
        {
            if (_weapons == null || _weapons.Count <= 0 || Enemies == null || Enemies.Count <= 0) return;

            foreach (var weapon in _weapons)
            {
                weapon.Rotate(GetNearestEnemy(weapon.transform.position, Enemies));
            }
        }

        public Vector3 GetNearestEnemy(Vector3 weaponPos, List<Transform> enemies)
        {
            if (_weapons == null || _weapons.Count <= 0) return Vector3.zero;

            Vector3 nearestEnemy = enemies[0].transform.position;
            foreach (var e in enemies)
            {
                if (Vector3.Distance(e.transform.position, weaponPos) < Vector3.Distance(nearestEnemy, weaponPos))
                {
                    nearestEnemy = e.transform.position;
                }
            }

            return nearestEnemy;
        }

        public void AddEnemyToFireRange(Transform transform)
        {
            Enemies.Add(transform);
        }

        public void RemoveEnemyToFireRange(Transform transform)
        {
            Enemies.Remove(transform);
        }
    }

}