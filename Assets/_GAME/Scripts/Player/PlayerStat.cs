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
            foreach (var item in statBuff.Stats)
            {
                GameService.LogColor($"Buff to Player: {item.TypeStat} {item.Value} {(item.TypeValueStat == TypeValueStat.Percentage ? "%" : "")}");
            }
            foreach (var stat in statBuff.Stats)
            {
                //Apply Value to Data
                CurrentStat.UpdateStat(GameService.CaculateStat(CurrentStat.GetStat(stat.TypeStat), stat,
                                            BaseData.GetStat(stat.TypeStat)));
            }
            ApplyStat();
            //foreach (var item in CurrentStat.Stats)
            //{
            //    GameService.LogColor($"After Buff to Player: {item.TypeStat} {item.Value} {(item.TypeValueStat == TypeValueStat.Percentage ? "%" : "")}");
            //}
            this.Dispatch<GameEvent.OnStatChange>(new GameEvent.OnStatChange());    
        }

        private void ApplyStat() => weaponCtrl.ApplyStat(currentStat);
        #endregion
    }

}