using UnityEngine;
namespace ShootingGame.Data {
    public enum WeaponType
    {
        SlashMellee = 0,
        ThrustMellee = 1,
        PitstolGun = 2,
        ShotGun = 3,
    }

    [CreateAssetMenu(fileName = "ItemAttributeData", menuName = "Items/AttributeData")]
    public class ItemAttributeData : AttributeData<AItem>
    {
        [SerializeField] private AItem prefab;
        [SerializeField] private StatContainerData stat;
        [SerializeField] private ItemAppearanceData appearance;
        [SerializeField] private MatrixData matrixData;
        [SerializeField] private ItemAttributeData nextLevelAttribute;


        public override AItem Prefab => prefab;
        public override StatContainerData Stat => stat;
        public override ItemAppearanceData Appearance => appearance;
        public MatrixData MatrixData => matrixData;
        public ItemAttributeData NextLevelAttribute => nextLevelAttribute;

    }
}