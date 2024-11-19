
using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "_GAME/PlayerData")]
    public class PlayerData : BaseIntKeyData<CharacterAttributeData>
    {
        protected override string Path => "Assets/_GAME/Prefabs/Character/Players";
    }
}