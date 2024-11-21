using UnityEngine;
namespace ShootingGame.Data {
    public interface IAppearanceData
    {
        string Name { get; }
        Sprite Icon { get; }
    }

    [System.Serializable]
    public class ItemAppearanceData : IAppearanceData
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite icon;

        public string Name => itemName;

        public Sprite Icon => icon;
    }
    
}