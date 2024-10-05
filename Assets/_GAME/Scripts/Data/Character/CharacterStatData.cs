
using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "CharacterStatData", menuName = "Items/CharacterStatData")]
    public class PlayerStatData : BaseIntKeyData<EquiqmentStat>
    {
        protected override string Path => throw new System.NotImplementedException();
    }
}
