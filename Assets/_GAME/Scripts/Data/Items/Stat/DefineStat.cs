using UnityEngine;

namespace ShootingGame.Data
{
    #region Struct Stat
    [System.Serializable]
    public enum TypeValueStat
    {
        FixedValue,
        Percentage
    }

    [System.Serializable]
    public enum TypeStat
    {
        Hp,
        Damage,
        AttackRate,
        Dodge,
        CritRate,
        Armor,
        DetectRange,
        MoveSpeed,
        Luck,
        DamageToHp,
        WeaponForce
    }
    [System.Serializable]
    public struct Stat
    {
        [SerializeField] private TypeStat typeStat;
        public TypeStat TypeStat => typeStat;
        [SerializeField] private TypeValueStat typeValueStat;
        public TypeValueStat TypeValueStat => typeValueStat;

        [SerializeField] private float value;
        public float Value => value;
        public void SetValue(float value) =>  this.value = value;

        public void SetTypeStat(TypeStat statType) => this.typeStat = statType;
        public void SetTypeValueStat(TypeValueStat type) => this.typeValueStat = type;

        public float GetValue(float baseValue = 1f)
        {
            if (typeValueStat == TypeValueStat.FixedValue)
            {
                return value;
            }
            else if (typeValueStat == TypeValueStat.Percentage)
            {
                return baseValue * (value / 100f);
            }
            return 0;
        }

        public Stat(Stat target)
        {
            this.typeStat = target.typeStat;
            this.value = target.value;
            this.typeValueStat = target.TypeValueStat;
        }
    }
    #endregion

    #region Class Equiqment

    #region Struct Design Stat
    public interface IStatProvider
    {
        public StatContainerData StatData { get; }
    }
    #endregion

    #region Stat Data
    // [System.Serializable]
    // public struct EquiqmentStat : IStatProvider
    // {
    //     [SerializeField] private StatContainerData data;
    //     public StatContainerData Data { get => data; }

    //     public EquiqmentStat(StatContainerData data)
    //     {
    //         this.data = new StatContainerData(data);
    //     }

    //     public EquiqmentStat Clone()
    //     {
    //         return new EquiqmentStat()
    //         {
    //             data = new StatContainerData(data)
    //         };
    //     }
    // }
    #endregion

    #endregion

}
