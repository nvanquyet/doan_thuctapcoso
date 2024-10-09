using UnityEngine;
namespace ShootingGame.Data {
    public enum WeaponType
    {
        SlashMellee = 0,
        ThrustMellee = 1,
        PitstolGun = 2,
        ShotGun = 3,
    }
    [CreateAssetMenu(fileName = "WeaponAttributeData", menuName = "Items/Weapon/WeaponAttributeData")]
    public class WeaponAttributeData : ScriptableObject
    {
        [SerializeField] private WeaponType weaponType;
        public WeaponType WeaponType => weaponType;
        [SerializeField] private EquiqmentStat itemAttributes;
        public EquiqmentStat ItemAttributes => itemAttributes;

        [SerializeField] private WeaponVisualData visualAttribute;
        public WeaponVisualData VisualAttribute => visualAttribute;
    }
}