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
    public struct Stat{
        [SerializeField] private float value;
        public float Value => value;
        [SerializeField] private StatType statType;
        public StatType StatType => statType;

        public Stat(float valueStat = 0f, StatType statType = StatType.FixedValue)
        {
            this.value = valueStat;
            this.statType = statType;
        }

        public void SetValue(float value){
            this.value = value;
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
public struct EquiqmentStat{
    public Stat damage;
    public Stat hp;
    public Stat attackSpeed;
    public Stat dodge;
    public Stat critRate;
    public Stat armor;
    public Stat range;
    public Stat moveSpeed;
    public Stat luck;
    
    // New stats
    public Stat hpRegen;
    public Stat damageToHp;
    public Stat damageToArmor;  
}


[System.Serializable]
public struct Equiqment
{
    public string itemName;
    public EquiqmentStat stats;
}

#endregion

}
