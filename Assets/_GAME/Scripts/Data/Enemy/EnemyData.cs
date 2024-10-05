using UnityEngine;
namespace ShootingGame.Data
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "_GAME/EnemyData")]
    public class EnemyData : BaseIntKeyData<ShootingGame.Enemy>
    {
        protected override string Path => "Assets/_GAME/Prefabs/Character/Enemies";
    }
}