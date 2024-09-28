
using UnityEngine;
namespace VawnWuyest.Data
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Items/Weapon/WeaponData")]
    public class WeaponData : BaseIntKeyData<BaseWeaponData>
    {
        protected override string Path => throw new System.NotImplementedException();
    }
    public abstract class BaseWeaponData : BaseIntKeyData<ItemAttributes>
    {

    }
}
