using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ShootingGame.Data;
namespace ShootingGame
{
    public class WeaponCtrl : MonoBehaviour
    {
        [SerializeField] private List<Transform> _allPositionSpawnWeapon;
        [SerializeField] private List<AWeapon> _weapons;
        [SerializeField] private List<AWeapon> _showWeapons;

        //public List<Transform> Enemies = new List<Transform>();

        private int MaxWeapon => _allPositionSpawnWeapon.Count;
#if UNITY_EDITOR
        private void OnValidate()
        {
            _allPositionSpawnWeapon = GetComponentsInChildren<Transform>().ToList();
            _allPositionSpawnWeapon.RemoveAt(0);
        }

#endif

        private void Start()
        {
            var index = 0;
            foreach (var weapon in _showWeapons)
            {
                var clone = Instantiate(weapon, _allPositionSpawnWeapon[index++]);
                clone.gameObject.SetActive(true);
                //weapon.gameObject.SetActive(false);
                _weapons.Add(clone);
            }
            
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

        public void AddWeapon(AWeapon newWeapon)
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
            if (_weapons == null || _weapons.Count <= 0) return;
            var enemies = LevelSpawner.Instance.GetActiveEnemies();
            foreach (var weapon in _weapons)
            {
                weapon.Rotate(GetNearestEnemy(weapon.transform.position, enemies));
            }
        }

        public Vector3 GetNearestEnemy(Vector3 weaponPos, List<Transform> enemies)
        {
            if (_weapons == null || _weapons.Count <= 0) return Vector3.zero;
            // FireRange At Here
            float distance = 80;
            if(enemies == null || enemies.Count <= 0) return Vector3.zero; 
            Vector3 nearestEnemy = enemies[0].transform.position;
            foreach (var e in enemies)
            {
                if (Vector3.Distance(e.transform.position, weaponPos) <= distance)
                {
                    nearestEnemy = e.transform.position;
                    distance = Vector3.Distance(nearestEnemy, weaponPos);
                }
            }

            return nearestEnemy;
        }

        //public void AddEnemyToFireRange(Transform transform)
        //{
        //    Enemies.Add(transform);
        //}

        //public void RemoveEnemyToFireRange(Transform transform)
        //{
        //    Enemies.Remove(transform);
        //}

        public void ApplyStat(IStatProvider stat)
        {
            if (_weapons == null || _weapons.Count <= 0) return;

            foreach (var weapon in _weapons)
            {
                weapon.ApplyStat(stat);
            }
        }
    }

}