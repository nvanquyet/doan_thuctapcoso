using UnityEngine;
using ShootingGame.Data;
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
        public void ResetStat() => currentStat = new StatContainerData(BaseData);

        public void BuffStat(StatContainerData statBuff)
        {
            if (statBuff == null) return;
            if (statBuff.Stats.Length <= 0) return;
            foreach (var stat in statBuff.Stats)
            {
                //Apply Value to Data
                CurrentStat.UpdateStat(GameService.CaculateStat(CurrentStat.GetStat(stat.TypeStat), stat,
                                            BaseData.GetStat(stat.TypeStat)));
            }
            this.Dispatch<GameEvent.OnStatChange>(new GameEvent.OnStatChange());    
        }

        public void ApplyStat() => weaponCtrl.ApplyStat(currentStat);
        #endregion
    }

}