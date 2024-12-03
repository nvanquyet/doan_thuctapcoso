
using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "_GAME/ItemData")]
    public class ItemData : BaseIntKeyData<ItemDataSO>
    {
        protected override string Path => throw new System.NotImplementedException();
    }
}