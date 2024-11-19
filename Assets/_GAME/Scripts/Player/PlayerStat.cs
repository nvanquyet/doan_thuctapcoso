using UnityEngine;
using ShootingGame.Data;
using Mono.CSharp;
using System;
namespace ShootingGame
{
    public class PlayerStat : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private WeaponCtrl weaponCtrl;

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


        void Start() {
            this.AddListener<GameEvent.OnNextWave>(OnNextWave);
        }

        private void OnNextWave(GameEvent.OnNextWave wave)
        {
            Invoke(nameof(ApplyStat), 0.1f);
        }

        #region Stat Ctrl
        public void BuffStat(StatContainerData statBuff)
        {
            if (statBuff == null) return;
            var allStats = statBuff.Stats;
            if (allStats == null || allStats.Length <= 0) return;
            foreach (var stat in allStats)
            {
                //Apply Value to Data
                CurrentStat.UpdateStat(GameService.CaculateStat(CurrentStat.GetStat(stat.TypeStat), stat,
                                            BaseData.GetStat(stat.TypeStat)));
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