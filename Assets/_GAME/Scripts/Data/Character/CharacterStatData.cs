
using UnityEngine;
namespace VawnWuyest.Data
{
    [CreateAssetMenu(fileName = "CharacterStatData", menuName = "Items/CharacterStatData")]
    public class PlayerStatData : BaseIntKeyData<PlayerStat>
    {
        protected override string Path => throw new System.NotImplementedException();
    }
}
