using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace VawnWuyest.Data
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

        public float GetValue(float baseValue)
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

    }
    #endregion

    #region Class Equiqment


    #region Base
    /// <summary>
    /// This is base stat that hold all Data Stat
    /// </summary>
    [System.Serializable]
    public class BaseStat
    {
        [SerializeField] private Stat[] datas;
        private Dictionary<TypeStat, Stat> dictDatas = new Dictionary<TypeStat, Stat>();

        public BaseStat(Stat[] datas)
        {
            this.datas = datas;
            InitDict();
        }

        public BaseStat(Stat[] datas, bool resetValue)
        {
            this.datas = datas;
            InitDict();
            if (!resetValue) return;
            foreach (Stat stat in datas) stat.SetValue(0);
        }

        public Stat[] AllStats => datas;

        public Stat GetStat(TypeStat type)
        {
            if (dictDatas == null || dictDatas.Count <= 0)
            {
                InitDict();
            }
            if (dictDatas.ContainsKey(type))
            {
                return dictDatas[type];
            }
            var stat = new Stat();
            stat.SetTypeStat(type);
            return stat;
        }

        private void InitDict()
        {
            if (dictDatas == null || dictDatas.Count <= 0)
            {
                dictDatas = new Dictionary<TypeStat, Stat>();
            }
            foreach (var property in datas)
            {
                dictDatas.Add(property.TypeStat, property);
            }
        }

    }
    #endregion

    #region Struct Design Stat
    public interface IStats
    {
        public BaseStat Data { get; }
    }
    #endregion

    #region Stat Data
    [System.Serializable]
    public struct EquiqmentStat : IStats
    {
        [SerializeField] private BaseStat data;
        public BaseStat Data { get => data; }
    }

    [System.Serializable]
    public struct WeaponStat : IStats
    {
        public string itemName;

        [SerializeField] private BaseStat data;
        public BaseStat Data { get => data; }
    }
    #endregion

    #endregion

}
