using System.Collections.Generic;
using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "StatContainerData", menuName = "Items/Stat/StatContainerData")]
    public class StatContainerData : ScriptableObject
    {
        [SerializeField] private Stat[] stats;
        public Stat[] Stats => stats;

        private Dictionary<TypeStat, Stat> dictDatas = new Dictionary<TypeStat, Stat>();

        public Dictionary<TypeStat, Stat> DictDatas {
            get {
                if(dictDatas == null || dictDatas.Count <= 0)
                {
                    InitDict();
                }
                return dictDatas;
            }
        }

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

            for (int i = 0; i < stats.Length; i++)
            {
                if (stats[i].TypeStat == updatedStat.TypeStat)
                {
                    stats[i] = updatedStat;
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
            foreach (var property in stats)
            {
                dictDatas.Add(property.TypeStat, property);
            }
        }

        public StatContainerData(StatContainerData clone)
        {
            stats = new Stat[clone.Stats.Length];
            for (int i = 0; i < clone.Stats.Length; i++)
            {
                stats[i] = new Stat(clone.Stats[i]);
            }
        }
    }
}
