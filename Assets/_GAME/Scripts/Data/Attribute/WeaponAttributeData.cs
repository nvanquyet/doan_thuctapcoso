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
    public class WeaponAttributeData : AttributeData<AWeapon, WeaponAppearanceData>
    {
        [SerializeField] private AWeapon prefab;
        [SerializeField] private StatContainerData stat;
        [SerializeField] private WeaponAppearanceData appearance;
        [SerializeField] private MatrixData matrixData;
        public override AWeapon Prefab => prefab;
        public override StatContainerData Stat => stat;
        public override WeaponAppearanceData Appearance => appearance;
        public MatrixData MatrixData => matrixData;
    }
}