using UnityEngine;
using ShootingGame.Data;
namespace ShootingGame
{
    public class PlayerStat : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private WeaponCtrl weaponCtrl;

        private EquiqmentStat baseData;
        public EquiqmentStat BaseData
        {
            get
            {
                if(baseData.Data == null)
                {
                    baseData = GameData.Instance.PlayerStatData.GetValue(id);
                }
                return baseData;
            }
        }
        private EquiqmentStat currentStat;
        public EquiqmentStat CurrentStat
        {
            get
            {
                if(currentStat.Data == null || currentStat.Data.Stats == null || currentStat.Data.Stats.Length <= 0)
                {
                    currentStat = BaseData.Clone();
                    ApplyStat();
                }
                return currentStat;
            }
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            weaponCtrl = GetComponentInChildren<WeaponCtrl>();
        }
#endif


        #region Stat Ctrl
        public void BuffStat(IStatProvider statBuff)
        {
            if (statBuff == null) return;
            var allStats = statBuff.Data.Stats;
            if (allStats == null || allStats.Length <= 0) return;
            foreach (var stat in allStats)
            {
                var current = CurrentStat.Data.GetStat(stat.TypeStat);
                var b = BaseData.Data.GetStat(stat.TypeStat);
                if (current.TypeValueStat == TypeValueStat.FixedValue) current.SetValue(current.Value + stat.GetValue(b.Value));
                else if (stat.TypeValueStat == TypeValueStat.FixedValue)
                {
                    current.SetValue(stat.Value + current.GetValue(stat.Value));
                    current.SetTypeValueStat(stat.TypeValueStat);
                }
                else
                {
                    current.SetValue(current.Value + stat.Value);
                }
                //Apply Value to Data
                CurrentStat.Data.UpdateStat(current);
            }
            ApplyStat();
            this.Dispatch<GameEvent.OnStatChange>(new GameEvent.OnStatChange());    
        }

        private void ApplyStat()
        {
            weaponCtrl.ApplyStat(currentStat);
        }

        #endregion
    }

}