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
public struct Equiqment
{
    public string itemName;
    public Stat[] stats;
}

#endregion

}
