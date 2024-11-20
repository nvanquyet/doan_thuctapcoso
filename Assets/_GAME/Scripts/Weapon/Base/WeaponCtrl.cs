using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ShootingGame.Data;
namespace ShootingGame
{
    public class WeaponCtrl : MonoBehaviour
    {
        [SerializeField] private List<WeaponSpawnPos> _allPositionSpawnWeapon;
        [SerializeField] private List<AWeapon> _weapons;

        //public List<Transform> Enemies = new List<Transform>();

        private int MaxWeapon => _allPositionSpawnWeapon.Count;
#if UNITY_EDITOR
        private void OnValidate()
        {
            _allPositionSpawnWeapon = GetComponentsInChildren<WeaponSpawnPos>().ToList();
        }

#endif

        private void Start()
        {
            this.AddListener<GameEvent.OnNextWave>(OnNextWave);   
        }

        private void OnNextWave(GameEvent.OnNextWave param)
        {
            var index = 0;
            var allItem = GameData.Instance.ItemData;
            //Remove all old weapon
            foreach (var w in _weapons) {
                Destroy(w.gameObject);
            }
            GameService.ClearList(ref _weapons);
            foreach (var i in param.allIDItem)
            {
                var data = allItem.GetValue(i);
                if (data.Prefab == null || !(data.Prefab is AWeapon)) continue;
                var clone = Instantiate(data.Prefab, _allPositionSpawnWeapon[index++].transform);
                
                clone.InitializeItem(data);
                clone.gameObject.SetActive(true);
                if (clone is AWeapon) _weapons.Add(clone as AWeapon);
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
                newWeapon.transform.SetParent(_allPositionSpawnWeapon[_weapons.Count].transform);
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

        public void ApplyStat(StatContainerData stat)
        {
            if (_weapons == null || _weapons.Count <= 0) return;

            foreach (var weapon in _weapons)
            {
                weapon.ApplyStat(stat);
            }
        }



        #region  Test
        public List<AWeapon> AllWeapons => _weapons;
        #endregion
    }

}