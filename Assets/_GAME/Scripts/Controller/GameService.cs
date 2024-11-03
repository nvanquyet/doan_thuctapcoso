using System;
using System.Collections.Generic;
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


        public static void ClearList<T>(ref List<T> list)
        {
            if (list == null) list = new();
            else list.Clear();
        }

        internal static void LogColor(string v)
        {
            Debug.Log($"<color=yellow>{v}</color>");
        }

        public static T ApplyWeightToValue<T>(T value, float weight){
            return (T)Convert.ChangeType(value, typeof(T)) switch
            {
                int intValue => (T)Convert.ChangeType(intValue * weight, typeof(T)),
                float floatValue => (T)Convert.ChangeType(floatValue * weight, typeof(T)),
                _ => value
            };
        }

        public static T ApplyScaleFactorToValue<T>(T value, float scaleFactor){
            return (T)Convert.ChangeType(value, typeof(T)) switch
            {
                int intValue => (T)Convert.ChangeType(Mathf.Pow(intValue , scaleFactor), typeof(T)),
                float floatValue => (T)Convert.ChangeType(Mathf.Pow(floatValue , scaleFactor), typeof(T)),
                _ => value
            };
        }

    }

}