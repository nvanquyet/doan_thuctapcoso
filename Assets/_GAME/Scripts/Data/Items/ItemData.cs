
using UnityEngine;
namespace VawnWuyest.Data
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Items/ItemData")]
    public class ItemData : BaseIntKeyData<ItemAttributes>
    {
        protected override string Path => throw new System.NotImplementedException();
    }
}
