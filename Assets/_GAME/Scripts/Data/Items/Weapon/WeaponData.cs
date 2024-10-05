
using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Items/Weapon/WeaponData")]
    public class WeaponData : BaseIntKeyData<WeaponAttributeData>
    {
        protected override string Path => throw new System.NotImplementedException();
    }
}
