using UnityEngine;
namespace ShootingGame.Data {
    public interface IAppearanceData
    {
        string Name { get; }
        Sprite Icon { get; }
    }

    [CreateAssetMenu(fileName = "ItemAppearanceData", menuName = "Items/ItemAppearanceData")]
    public class ItemAppearanceData : ScriptableObject, IAppearanceData
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite icon;

        public string Name => itemName;

        public Sprite Icon => icon;
    }
    
}