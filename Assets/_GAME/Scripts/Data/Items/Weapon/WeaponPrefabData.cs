using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "WeaponPrefabData", menuName = "Items/Weapon/WeaponPrefabData")]
    public class WeaponPrefabData : BaseIntKeyData<AWeapon>
    {
        protected override string Path => throw new System.NotImplementedException();
    }
}
