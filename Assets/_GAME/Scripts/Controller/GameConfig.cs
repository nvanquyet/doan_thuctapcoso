using UnityEngine;

namespace ShootingGame
{
    public enum SceneIndex
    {
        Login = 0,
        Home = 1,
        InGame = 2
    }

    [CreateAssetMenu(fileName = "GameConfig", menuName = "_GAME/GameConfig")]
    public class GameConfig : SingletonResourcesScriptable<GameConfig>
    {
        public float ScalingFactor = 1.1f;

        public int BaseCost = 10;

        public EnemyWeights enemyWeights;

        public int bossWaveDistance = 5;

        public WaveProperties waveProperties;


        public bool startGameBuyItem = true;

        public int StartCoin = 100;

        public float EnergyGainInterval = 7200;
        public int MaxEnergy = 5;

        protected override void Initialize()
        {
            
        }
    }

}