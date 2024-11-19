using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "StatData", menuName = "Items/Stat/StatData")]
    public class StatData : ScriptableObject
    {
        [SerializeField] private Stat stat;
        public Stat Stat => stat;

        public void UpdateStat(Stat updatedStat)
        {
            stat = updatedStat;
        }

        public StatData(Stat stat)
        {
            this.stat = stat;
        }
    }
}
