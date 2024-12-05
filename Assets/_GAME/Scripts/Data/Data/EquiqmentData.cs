using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "EquiqmentData", menuName = "Items/List/Equiqment")]
    public class EquiqmentData : BaseIntKeyData<ItemEquiqmentData>
    {
        protected override string Path => "Assets/_GAME/Data/Item/Equiqment";
    }
}
