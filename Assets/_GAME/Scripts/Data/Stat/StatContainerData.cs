using System.Collections.Generic;
using UnityEngine;
namespace ShootingGame.Data
{
    [System.Serializable]
    public class StatContainerData
    {
        [SerializeField] private StatData[] stats;

        private Stat[] baseStat;
        public Stat[] Stats
        {
            get
            {
                var result = new Stat[stats.Length];
                for (int i = 0; i < stats.Length; i++)
                {
                    result[i] = stats[i].Stat;
                }
                if (baseStat == null || baseStat.Length <= 0)
                {
                    baseStat = result;
                }
                return result;
            }
        }

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

            for (int i = 0; i < Stats.Length; i++)
            {
                if (stats[i].Stat.TypeStat == updatedStat.TypeStat)
                {
                    stats[i].UpdateStat(updatedStat);
                    GameService.LogColor($"Update Stat {updatedStat.TypeStat} {stats[i].Stat.Value}");
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
            if(stats == null) return;
            foreach (var property in Stats)
            {
                if(!dictDatas.ContainsKey(property.TypeStat)) dictDatas.Add(property.TypeStat, property);
                else dictDatas[property.TypeStat] = property;
            }
        }

        public StatContainerData(StatContainerData clone)
        {
            if(clone == null) return;
            stats = new StatData[clone.Stats.Length];
            for (int i = 0; i < clone.Stats.Length; i++)
            {
                var newStat = new Stat(clone.Stats[i]);
                if (stats[i] == null) stats[i] = new StatData(newStat);
                else stats[i].UpdateStat(newStat);
            }
        }
    }
}
