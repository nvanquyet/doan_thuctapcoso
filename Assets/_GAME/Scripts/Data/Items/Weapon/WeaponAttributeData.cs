using UnityEngine;
using ShootingGame.Data;
namespace ShootingGame.Data {
    [CreateAssetMenu(fileName = "WeaponAttributeData", menuName = "Items/Weapon/WeaponAttributeData")]
    public class WeaponAttributeData : ScriptableObject
    {
        [SerializeField] private EquiqmentStat itemAttributes;
        public EquiqmentStat ItemAttributes => itemAttributes;
    }
}