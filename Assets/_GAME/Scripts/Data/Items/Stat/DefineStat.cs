using System;
using System.Collections.Generic;
using System.Linq;
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
        Hp,
        Damage,
        AttackSpeed,
        Dodge,
        CritRate,
        Armor,
        Range,
        MoveSpeed,
        Luck,
        DamageToHp,
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

        public Stat(Stat target)
        {
            this.typeStat = target.typeStat;
            this.value = target.value;
            this.typeValueStat = target.TypeValueStat;
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
            this.datas = datas.Select(stat => new Stat(stat)).ToArray();
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

        public void UpdateStat(Stat updatedStat)
        {
            if (dictDatas == null || dictDatas.Count <= 0)
            {
                InitDict();
            }
            if (dictDatas.ContainsKey(updatedStat.TypeStat))
            {
                dictDatas[updatedStat.TypeStat] = updatedStat;
            }
            else
            {
                dictDatas.Add(updatedStat.TypeStat, updatedStat);
            }

            for (int i = 0; i < datas.Length; i++)
            {
                if (datas[i].TypeStat == updatedStat.TypeStat)
                {
                    datas[i] = updatedStat;
                    return;
                }
            }
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

        public EquiqmentStat(Stat[] data)
        {
            this.data = new BaseStat(data);
        }
    }

    [System.Serializable]
    public struct ItemStat : IStats
    {
        public string itemName;

        [SerializeField] private BaseStat data;
        public BaseStat Data { get => data; }
    }
    #endregion

    #endregion

}
