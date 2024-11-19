using UnityEngine;
namespace ShootingGame.Data {
    [System.Serializable]
    public struct WeaponAppearanceStruct : IAppearanceData
    {
        [SerializeField] private string itemName;
        public string ItemName => itemName;
        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;
        [SerializeField] private string description;
        public string Description => description;

    }
    [CreateAssetMenu(fileName = "WeaponVisualData", menuName = "Items/Weapon/WeaponVisualData")]
    public class WeaponAppearanceData : ItemAppearanceData<WeaponAppearanceStruct>
    {

    }
}