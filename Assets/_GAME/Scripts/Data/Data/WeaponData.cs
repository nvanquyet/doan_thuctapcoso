using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Items/List/Weapons")]
    public class WeaponData : BaseIntKeyData<ItemWeaponData>
    {
        protected override string Path => "Assets/_GAME/Data/Item/Weapon";
    }
}
