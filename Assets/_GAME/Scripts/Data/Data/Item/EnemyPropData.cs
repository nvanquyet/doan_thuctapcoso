using UnityEngine;
namespace ShootingGame
{
    [System.Serializable]

    public struct EnemyPropertiesStruct
    {
        public short BaseHealth;
        public short BaseDamage;
        public short BaseSpeed;
        public int BaseEXP;
        public int BaseCoin;
        public float GrowthRate;
    }


    [CreateAssetMenu(fileName = "EnemyProp", menuName = "_GAME/Enemy Prop")]
    public class EnemyPropData : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private EnemyPropertiesStruct properties;
        [SerializeField] private Enemy prefabs;
        public EnemyPropertiesStruct Properties => properties;
        public Sprite Icon => icon;
        public Enemy Prefabs => prefabs;
    }
}
