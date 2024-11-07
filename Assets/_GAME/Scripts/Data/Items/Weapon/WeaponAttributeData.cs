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
    public class WeaponAttributeData : ScriptableObject, IStatProvider
    {
        [SerializeField] private WeaponType weaponType;
        public WeaponType WeaponType => weaponType;
        [SerializeField] private StatContainerData itemAttributes;
        public StatContainerData StatData => itemAttributes;
        //[SerializeField] private EquiqmentStat itemAttributes;
        //public EquiqmentStat ItemAttributes => itemAttributes;

        [SerializeField] private WeaponVisualData visualAttribute;
        private Sprite icon;

        // Correct definition of Icon as a property (not a method)
        public Sprite Icon => icon;

        public WeaponVisualData VisualAttribute => visualAttribute;
        public void SetIcon(Sprite newIcon)
        {
            icon = newIcon;
        }
    }
}