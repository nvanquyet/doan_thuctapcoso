using UnityEngine;
namespace ShootingGame.Data {
    public interface IAppearanceData
    {
        string ItemName { get; }
        Sprite Icon { get; }
        string Description { get; }
    }
    
    public class ItemAppearanceData<T> : ScriptableObject where T : IAppearanceData
    {
        [SerializeField] protected T itemVisual;
        public T GetVisual()
        {
            return itemVisual;
        }
    }
    [System.Serializable]
    public struct CharacterAppearanceStruct : IAppearanceData
    {
        [SerializeField] private string itemName;
        public string ItemName => itemName;
        [SerializeField] private Sprite icon;
        public Sprite Icon => icon;
        [SerializeField] private string description;
        public string Description => description;

    }
    [CreateAssetMenu(fileName = "CharacterAppearanceData", menuName = "Character/CharacterAppearanceData")]
    public class CharacterAppearanceData : ItemAppearanceData<CharacterAppearanceStruct>
    {

    }
}