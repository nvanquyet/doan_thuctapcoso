using UnityEngine;

namespace ShootingGame.Data
{
    public abstract class AttributeData<T, K> : ScriptableObject, IStatProvider where T : MonoBehaviour
    {
        public abstract T Prefab { get; }
        public abstract StatContainerData Stat { get; }
        public abstract K Appearance { get; }
    }

    [CreateAssetMenu(fileName = "CharacterAttributeData", menuName = "Character/CharacterAttributeData")]
    public class CharacterAttributeData : AttributeData<Player, CharacterAppearanceData>
    {
        [SerializeField] private Player prefab;
        [SerializeField] private StatContainerData stat;
        [SerializeField] private CharacterAppearanceData appearance;

        public override Player Prefab => prefab;
        public override StatContainerData Stat => stat;
        public override CharacterAppearanceData Appearance => appearance;
    }
}
