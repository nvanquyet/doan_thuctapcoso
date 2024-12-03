using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ShootingGame.Data;
using System;
namespace ShootingGame
{
    public class WeaponCtrl : MonoBehaviour
    {
        [SerializeField] private List<WeaponSpawnPos> _allPositionSpawnWeapon;
        [SerializeField] private List<AWeapon> _weapons;
        [SerializeField] private Player _player;

        //Note: Vector3 is local Position of weapon in WeaponCtrl
        private Dictionary<AWeapon, Vector3> dictWeaponPos;
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
            //Clear dict
            if (dictWeaponPos != null && dictWeaponPos.Count > 0)
            {
                dictWeaponPos.Clear();
            }
            GameService.ClearList(ref _weapons);
        }

        private void OnNextWave(GameEvent.OnNextWave param)
        {
            if (dictWeaponPos == null) dictWeaponPos = new Dictionary<AWeapon, Vector3>();
            else dictWeaponPos.Clear();

            _player.Stat.ResetStat();

            foreach (var i in param.allEquiqments)
            {
                _player.Stat.BuffStat(i.Stat);
            }

            int index = 0;
            foreach (var w in param.allWeapons)
            {
                if (w.Prefab != null && (w.Prefab is AWeapon))
                {
                    var tsWp = _allPositionSpawnWeapon[index].transform;
                    var clone = Instantiate(w.Prefab, tsWp);
                    clone.InitializeItem(w);
                    clone.gameObject.SetActive(true);
                    if (clone is AWeapon)
                    {
                        _weapons.Add(clone as AWeapon);
                        dictWeaponPos.Add(clone as AWeapon, tsWp.localPosition);
                        clone.ApplyStat(_player.Stat.CurrentStat);
                    }
                    index++;
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

        public void Rotate()
        {
            if (_weapons == null || _weapons.Count <= 0) return;
            var enemies = LevelSpawner.Instance.GetActiveEnemies();
            foreach (var weapon in _weapons)
            {
                var neareatEnemy = GetNearestEnemy(weapon, enemies);
                if(neareatEnemy != null)
                {
                    weapon.SetTarget(neareatEnemy);
                    weapon.Rotate(neareatEnemy.transform.position);
                }
            }
        }

        public Transform GetNearestEnemy(AWeapon weapon, List<Transform> enemies)
        {
            if (_weapons == null || _weapons.Count <= 0) return null;
            // FireRange At Here
            float distance = 80;
            if (enemies == null || enemies.Count <= 0) return null;
            var nearestEnemy = enemies[0];
            var weaponPos = transform.position + dictWeaponPos[weapon];
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
    }

}