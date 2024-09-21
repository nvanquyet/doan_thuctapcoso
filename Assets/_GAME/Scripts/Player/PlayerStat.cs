using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VawnWuyest.Data;
namespace ShootingGame
{
    public class PlayerStat : MonoBehaviour
    {
        [SerializeField] private PlayerStatData baseData;
        [SerializeField] private WeaponCtrl weaponCtrl;
        
        private IStats currentStat;
        public IStats CurrentStat => currentStat;

        #if UNITY_EDITOR
        private void OnValidate()
        {
            weaponCtrl = GetComponentInChildren<WeaponCtrl>();
        }
        #endif


        private void Start()
        {
            currentStat = baseData.GetAllValue()[0];
            ApplyStat();
        }

        

        #region Stat Ctrl
        public void BuffStat(IStats statBuff){
            if(statBuff == null) return;
            var allStats = statBuff.Data.AllStats;
            if(allStats == null || allStats.Length <= 0) return;
            foreach (var stat in allStats)
            {
                var current = currentStat.Data.GetStat(stat.TypeStat);
                var baseValue = baseData.GetAllValue()[0].Data.GetStat(stat.TypeStat).Value;
                if(current.TypeValueStat == stat.TypeValueStat &&
                    current.TypeValueStat == TypeValueStat.Percentage)
                {
                    current.SetValue(current.Value + stat.Value);
                }else {
                    current.SetValue(current.Value + stat.GetValue(baseValue));
                }
            }
            ApplyStat();
        }

        private void ApplyStat(){
            weaponCtrl.ApplyStat(currentStat);
        }

        #endregion
    }

}