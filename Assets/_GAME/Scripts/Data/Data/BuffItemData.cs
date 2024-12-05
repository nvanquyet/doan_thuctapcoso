using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "BuffItem", menuName = "Items/List/BuffItems")]
    public class BuffItemData : BaseIntKeyData<ItemBuffData>
    {
        protected override string Path => throw new System.NotImplementedException();
    }
}
