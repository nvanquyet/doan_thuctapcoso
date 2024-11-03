using UnityEngine;

namespace ShootingGame
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "_GAME/GameConfig")]
    public class GameConfig : SingletonResourcesScriptable<GameConfig>
    {
        public float scalingFactor = 1.1f;
        public EnemyWeights enemyWeights;
        public int bossWaveDistance = 5;

        public WaveProperties waveProperties;

        protected override void Initialize()
        {
            
        }
    }

}