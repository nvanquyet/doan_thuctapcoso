
using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "PlayerSO", menuName = "_GAME/PlayerData")]
    public class PlayerData : BaseIntKeyData<ShootingGame.Player>
    {
        protected override string Path => "Assets/_GAME/Prefabs/Character/Players";
    }
}