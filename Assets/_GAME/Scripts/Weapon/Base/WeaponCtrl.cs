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
        [SerializeField] private Player _player;

        //public List<Transform> Enemies = new List<Transform>();

        private int MaxWeapon => _allPositionSpawnWeapon.Count;
#if UNITY_EDITOR
        [SerializeField] private TestStat testStat;
        private void OnValidate()
        {
            _allPositionSpawnWeapon = GetComponentsInChildren<WeaponSpawnPos>().ToList();
            _player = GetComponentInParent<Player>();
            testStat = FindObjectOfType<TestStat>();
        }

#endif

        private void Start()
        {
            this.AddListener<GameEvent.OnNextWave>(OnNextWave);
            this.AddListener<GameEvent.OnWaveClear>(OnWaveClear);
        }


        private void OnWaveClear(GameEvent.OnWaveClear param)
        {
            //Remove all old weapon
            foreach (var w in _weapons)
            {
                Destroy(w.gameObject);
            }
            GameService.ClearList(ref _weapons);
        }

        private void OnNextWave(GameEvent.OnNextWave param)
        {
            var index = 0;
            var allItems = GameData.Instance.ItemData;

            _player.Stat.ResetStat();
            var allIDWeapon = new List<int>();
            foreach (var i in param.allIDItem)
            {
                var data = allItems.GetValue(i);
                if (data == null) continue;
                if (data.Prefab != null && (data.Prefab is AWeapon)) allIDWeapon.Add(i);
                else _player.Stat.BuffStat(data.Stat);
            }
            if (allIDWeapon.Count > 0)
            {
                foreach (var wID in allIDWeapon)
                {
                    var data = allItems.GetValue(wID);
                    if (data.Prefab != null && (data.Prefab is AWeapon))
                    {
                        var clone = Instantiate(data.Prefab, _allPositionSpawnWeapon[index++].transform);
                        clone.InitializeItem(data);
                        clone.gameObject.SetActive(true);
                        if (clone is AWeapon) _weapons.Add(clone as AWeapon);
                        clone.ApplyStat(_player.Stat.CurrentStat);
                    }
                }
            }
#if UNITY_EDITOR
            testStat.ShowStat(_player);
#endif

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
                var neareatEnemy = GetNearestEnemy(weapon.transform.position, enemies);
                weapon.SetTarget(neareatEnemy);  
                weapon.Rotate(neareatEnemy.transform.position);
            }
        }

        public Transform GetNearestEnemy(Vector3 weaponPos, List<Transform> enemies)
        {
            if (_weapons == null || _weapons.Count <= 0) return null;
            // FireRange At Here
            float distance = 80;
            if (enemies == null || enemies.Count <= 0) return null;
            var nearestEnemy = enemies[0];
            foreach (var e in enemies)
            {
                if (Vector3.Distance(e.transform.position, weaponPos) <= distance)
                {
                    nearestEnemy = e;
                    distance = Vector3.Distance(nearestEnemy.transform.position, weaponPos);
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
    }

}