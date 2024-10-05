
using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/ItemData")]
    public class ItemData : BaseIntKeyData<EquiqmentStat>
    {
        protected override string Path => throw new System.NotImplementedException();
    }
}
