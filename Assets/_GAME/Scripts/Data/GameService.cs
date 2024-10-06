using ShootingGame.Data;
using UnityEngine;
namespace ShootingGame
{

    public class GameService 
    {
        public static Stat CaculateStat(Stat currentStat, Stat bonousStat, Stat baseStat)
        {
            if(bonousStat.Value == 0) return currentStat;
            if (currentStat.TypeValueStat == bonousStat.TypeValueStat) currentStat.SetValue(currentStat.Value + bonousStat.Value); 
            else
            {
                if (currentStat.TypeValueStat == TypeValueStat.FixedValue)
                {
                    currentStat.SetValue(baseStat.Value + bonousStat.GetValue(baseStat.Value));
                }else
                {
                    currentStat.SetValue(bonousStat.Value + currentStat.GetValue(bonousStat.Value));
                    currentStat.SetTypeValueStat(bonousStat.TypeValueStat);
                }
            }
            return currentStat;
        }
    }

}