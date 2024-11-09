
using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "CharacterStatData", menuName = "Items/CharacterStatData")]
    public class PlayerStatData : BaseIntKeyData<StatContainerData>
    {
        protected override string Path => throw new System.NotImplementedException();
    }
}
