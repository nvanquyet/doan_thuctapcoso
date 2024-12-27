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
        //Note: Vector3 is local Position of weapon in WeaponCtrl
        private Dictionary<AWeapon, Vector3> dictWeaponPos;

        public Dictionary<AWeapon, Vector3> DictWeaponPos
        {
            get
            {
                if (dictWeaponPos == null) dictWeaponPos = new Dictionary<AWeapon, Vector3>();
                return dictWeaponPos;
            }
        }
        private int MaxWeapon => _allPositionSpawnWeapon.Count;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _allPositionSpawnWeapon = GetComponentsInChildren<WeaponSpawnPos>().ToList();
            _player = GetComponentInParent<Player>();
        }

#endif

        private void Start()
        {
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
            if (DictWeaponPos != null && DictWeaponPos.Count > 0)
            {
                DictWeaponPos.Clear();
            }
            GameService.ClearList(ref _weapons);
        }

        public void InitWeapon(List<ItemWeaponData> param, StatContainerData currentStat)
        {
            int index = 0;
            var data = GameData.Instance.ItemData.GetValue(Category.Weapon) as WeaponData;
            foreach (var w in param)
            {
                if (w.Prefab != null && (w.Prefab is AWeapon weapon))
                {
                    var tsWp = _allPositionSpawnWeapon[index].transform;
                    var clone = Instantiate(weapon, tsWp);
                    clone.InitializeItem(w);
                    clone.gameObject.SetActive(true);
                    clone.ApplyStat(currentStat);
                    clone.SetReceiver(_player);
                    _weapons.Add(clone);
                    DictWeaponPos.Add(clone, tsWp.localPosition);

                    if(data != null && clone is RangedWeapon rangedWeapon)
                    {
                        rangedWeapon.InitProjectile(data.GetIndexOfValue(w));
                    }

                    index++;
                }
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

        public void Rotate()
        {
            if (_weapons == null || _weapons.Count <= 0) return;
            var enemies = LevelSpawner.Instance.GetActiveEnemies();
            if (enemies == null || enemies.Count <= 0) return;
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
            var weaponPos = transform.position + DictWeaponPos[weapon];
            foreach (var e in enemies)
            {
                if(e == null) continue;
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