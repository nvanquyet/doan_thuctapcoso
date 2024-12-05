using System;
using UnityEngine;
namespace ShootingGame.Data
{
    public enum LevelItem
    {
        Level1 = 1,
        Level2 = 2,
        Level3 = 3
    }
    [CreateAssetMenu(fileName = "ItemEquiqmentData", menuName = "Items/Single/ItemEquiqmentData")]
    public class ItemEquiqmentData : ItemDataSO
    {
        [SerializeField] private AItem prefab;
        [SerializeField] private LevelItem level;

        [SerializeField] private ItemDataSO nextLevelAttribute;

        public AItem Prefab => prefab;
        public ItemDataSO NextLevelAttribute => nextLevelAttribute;
        public LevelItem Level => level;

#if UNITY_EDITOR
        private void OnValidate()
        {
            var stringNameObject = this.name.Substring(this.name.IndexOf(" "));

            this.level = (LevelItem) Int32.Parse(stringNameObject[1].ToString());

        }
#endif
    }
}
