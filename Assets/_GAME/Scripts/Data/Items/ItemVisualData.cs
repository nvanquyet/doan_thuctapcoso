using UnityEngine;
namespace ShootingGame.Data {
    public interface IItemVisual
    {
        string ItemName { get; }
        Sprite Icon { get; }
        string Description { get; }
    }
    public class ItemVisualData<T> : ScriptableObject where T : IItemVisual
    {
        [SerializeField] protected T itemVisual;
        public K GetVisual<K>() where K : T
        {
            return (K)itemVisual;
        }
    }
}
