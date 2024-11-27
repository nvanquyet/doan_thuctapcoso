using UnityEngine;
using ShootingGame.Data;
using System;
namespace ShootingGame
{
    public class PlayerStat : MonoBehaviour
    {
        [SerializeField] private int id;

        private StatContainerData baseData;
        public StatContainerData BaseData
        {
            get
            {
                if(baseData == null)
                {
                    baseData = GameData.Instance.Players.GetValue(id).Stat;
                }
                return baseData;
            }
        }
        private StatContainerData currentStat;
        public StatContainerData CurrentStat
        {
            get
            {
                if(currentStat == null || currentStat.Stats == null || currentStat.Stats.Length <= 0)
                {
                    currentStat = new StatContainerData(BaseData);
                }
                return currentStat;
            }
        }

        public Action<StatContainerData> OnStatChanged;

        #region Stat Ctrl
        public void ResetStat() => currentStat = new StatContainerData(BaseData);

        public void BuffStat(StatContainerData statBuff)
        {
            if (statBuff == null) return;
            if (statBuff.Stats.Length <= 0) return;
            foreach (var stat in statBuff.Stats)
            {
                //Apply Value to Data
                var newStat = GameService.CaculateStat(CurrentStat.GetStat(stat.TypeStat), stat, BaseData.GetStat(stat.TypeStat));
                CurrentStat.UpdateStat(newStat);
            }
            OnStatChanged?.Invoke(CurrentStat);
        }
       #endregion
    }

}