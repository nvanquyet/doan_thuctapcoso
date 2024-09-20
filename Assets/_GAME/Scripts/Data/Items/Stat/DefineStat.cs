using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace VawnWuyest.Data
{
#region Struct Stat
    [System.Serializable]
    public enum StatType
    {
        FixedValue, 
        Percentage  
    }

    [System.Serializable]
    public enum StatDefine
    {
        Damage,
        Hp,
        AttackSpeed,
        Dodge,
        CritRate,
        Armor,
        Range,
        MoveSpeed,
        Luck,
        HpRegen,
        DamageToHp,
        DamageToArmor
    }
    [System.Serializable]
    public struct Stat{
        [SerializeField] private StatDefine statDefine;
        public StatDefine StatDefine => statDefine;
        [SerializeField] private StatType statType;
        public StatType StatType => statType;
        [SerializeField] private float value;
        public float Value => value;

        public void SetValue(float value){
            this.value = value;
        } 
        public void SetStatDefine(StatDefine statType){
            this.statDefine = statType;
        }
        
        public float GetValue(float baseValue)
        {
            if (statType == StatType.FixedValue)
            {
                return value; 
            }
            else if (statType == StatType.Percentage)
            {
                return baseValue * (value / 100f); 
            }
            return 0;
        }

    }
#endregion

#region Class Equiqment
[System.Serializable]
public class StatData
{
    [SerializeField] private Stat[] allProperties;
    private Dictionary<StatDefine, Stat> dictProperties = new Dictionary<StatDefine, Stat>();
    
    public StatData(Stat[] allProperties)
    {
        this.allProperties = allProperties;
        InitDict();
    }

    public Stat[] AllProperties => allProperties;

    public Stat GetStat(StatDefine type){
        if(dictProperties == null || dictProperties.Count <= 0){
            InitDict();
        }
        if(dictProperties.ContainsKey(type)){
            return dictProperties[type];
        }
        var stat = new Stat();
        stat.SetStatDefine(type);
        return stat;
    }

    private void InitDict(){
        if(dictProperties == null || dictProperties.Count <= 0){
            dictProperties = new Dictionary<StatDefine, Stat>();
        }
        foreach (var property in allProperties)
        {
            dictProperties.Add(property.StatDefine, property);
        }
    }

}


[System.Serializable]
public struct Equiqment
{
    public string itemName;
    public StatData allStats;
}
[System.Serializable]
public struct PlayerStat
{
    [SerializeField] private StatData allStats;

    private StatData bonousStats;

    public StatData AllStats => allStats;

    public StatData BonousStats {
        get {
            if(bonousStats == null || bonousStats.AllProperties.Length <= 0){
                var newStats = new Stat[AllStats.AllProperties.Length];
                for(int i = 0; i < AllStats.AllProperties.Length; i++){
                    newStats[i] = AllStats.AllProperties[i];
                }
                bonousStats = new StatData(newStats);
            }
            return bonousStats;
        }
    }
    
    public StatData TotalStats {
        get {
            Stat[] totalStat = new Stat[allStats.AllProperties.Length];
            for (int i = 0; i < AllStats.AllProperties.Length; i++)
            {
                var type = allStats.AllProperties[i].StatDefine;
                totalStat[i] = AllStats.AllProperties[i];
                var stat = AllStats.GetStat(type);
                var value = stat.Value + BonousStats.GetStat(type).GetValue(stat.Value);
                totalStat[i].SetValue(value);
            }
            return new StatData(totalStat);
        }
    }
}


#endregion

}
