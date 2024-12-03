using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "ItemEquiqmentData", menuName = "Items/ItemEquiqmentData")]
    public class ItemEquiqmentData : ItemDataSO
    {
        [SerializeField] private AItem prefab;


        [SerializeField] private ItemDataSO nextLevelAttribute;

        public AItem Prefab => prefab;
        public ItemDataSO NextLevelAttribute => nextLevelAttribute;
    }
}
