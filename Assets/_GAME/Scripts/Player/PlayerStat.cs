using UnityEngine;
using ShootingGame.Data;
using System;
namespace ShootingGame
{
    public class PlayerStat : MonoBehaviour
    {

        [SerializeField] private WeaponCtrl weaponCtrl;
        private Action<StatContainerData> OnStatChanged;
#if UNITY_EDITOR
        private void OnValidate()
        {
            weaponCtrl = GetComponentInChildren<WeaponCtrl>();
        }
#endif
        private StatContainerData baseData;
        private StatContainerData currentStat;

        public StatContainerData BaseData
        {
            get
            {
                if(baseData == null)
                {
                    baseData = GameData.Instance.Players.GetValue(UserData.CurrentCharacter).Stat;
                }
                return baseData;
            }
        }
        
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


        public void Initialized(Action<StatContainerData> OnStatChanged)
        {
            this.OnStatChanged = OnStatChanged;
            OnStatChanged?.Invoke(CurrentStat);
        }
        public void OnNextWave(GameEvent.OnNextWave param)
        {
            ResetStat();
            foreach (var i in param.allEquiqments)
            {
                BuffStat(i.Stat);
            }
            weaponCtrl.InitWeapon(param.allWeapons, CurrentStat);
        }
        #endregion
    }

}