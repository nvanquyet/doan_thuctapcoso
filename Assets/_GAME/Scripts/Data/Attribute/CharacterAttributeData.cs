using UnityEngine;

namespace ShootingGame.Data
{
    public abstract class AttributeData<T> : ScriptableObject, IStatProvider where T : MonoBehaviour
    {
        public abstract T Prefab { get; }
        public abstract StatContainerData Stat { get; }
        public abstract ItemAppearanceData Appearance { get; }

    }

    [CreateAssetMenu(fileName = "CharacterAttributeData", menuName = "Character/CharacterAttributeData")]
    public class CharacterAttributeData : AttributeData<Player>
    {
        [SerializeField] private Player prefab;
        [SerializeField] private StatContainerData stat;
        [SerializeField] private ItemAppearanceData appearance;

        public override Player Prefab => prefab;
        public override StatContainerData Stat => stat;
        public override ItemAppearanceData Appearance => appearance;
    }
}
