using System.Collections.Generic;
using UnityEngine;
namespace ShootingGame.Data
{
    [System.Serializable]
    public class StatContainerData
    {
        [SerializeField] private Stat[] stats;

        public Stat[] Stats => stats;

        private Dictionary<TypeStat, Stat> dictDatas;

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
            if (DictDatas.ContainsKey(type))
            {
                return DictDatas[type];
            }
            var stat = new Stat();
            stat.SetTypeStat(type);
            return stat;
        }

        public void UpdateStat(Stat updatedStat)
        {
            if (DictDatas.ContainsKey(updatedStat.TypeStat))
            {
                DictDatas[updatedStat.TypeStat] = updatedStat;
            }
            else
            {
                DictDatas.Add(updatedStat.TypeStat, updatedStat);
            }

            for (int i = 0; i < Stats.Length; i++)
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
            if(stats == null) return;
            dictDatas = new Dictionary<TypeStat, Stat>();
            foreach (var property in Stats)
            {
                if(!dictDatas.ContainsKey(property.TypeStat)) dictDatas.Add(property.TypeStat, property);
                else dictDatas[property.TypeStat] = property;
            }
        }

        public StatContainerData(StatContainerData clone)
        {
            if(clone == null || clone.Stats == null) return;
            stats = new Stat[clone.Stats.Length];
            for (int i = 0; i < clone.Stats.Length; i++)
            {
                stats[i] = new Stat(clone.Stats[i]);
            }
        }
    }
}
