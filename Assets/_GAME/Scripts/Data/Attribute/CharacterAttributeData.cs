using UnityEngine;

namespace ShootingGame.Data
{
    public enum CategoryItem
    {
        MeleeWeapon = 0,
        RangeWeapon = 1,
        Equipment = 2,
        Consumable = 3,
        Accessory = 4,
    }

    public enum RarityItem
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4,
    }
    public class ItemDataSO : ScriptableObject
    {
        [SerializeField] private CategoryItem category;
        [SerializeField] private RarityItem rarity;

        [SerializeField] private float costTierMultiplier = 1f;
        [SerializeField] private StatContainerData stat;
        [SerializeField] private ItemAppearanceData appearance;
        [SerializeField] private MatrixData matrixData;

        public StatContainerData Stat => stat;
        public ItemAppearanceData Appearance => appearance;
        public MatrixData MatrixData => matrixData;
        public RarityItem Rarity => rarity;
        public float CostTierMultiplier => costTierMultiplier;
    }


    [CreateAssetMenu(fileName = "CharacterAttributeData", menuName = "Character/CharacterAttributeData")]
    public class CharacterAttributeData : ScriptableObject
    {

        [SerializeField] private StatContainerData stat;
        [SerializeField] private RuntimeAnimatorController animator;
        [SerializeField] private ItemAppearanceData appearance;
        [SerializeField] private RarityItem rarity;
        public RuntimeAnimatorController Animator => animator;
        public ItemAppearanceData Appearance => appearance;
        public StatContainerData Stat => stat;
        public RarityItem Rarity => rarity;
    }
}
