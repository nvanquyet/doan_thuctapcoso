
using UnityEngine;
namespace ShootingGame.Data
{
    public enum Category
    {
        Weapon,
        Equiqment,
        BuffItem
    }
    [CreateAssetMenu(fileName = "ItemData", menuName = "_GAME/ItemData")]
    public class ItemData : BaseData<Category, ScriptableObject>
    {
        public override void OnValidateData()
        {
            
        }

        public override void OnValidateKey()
        {
            
        }

        public override void OnValidateValue()
        {
            
        }
    }
}