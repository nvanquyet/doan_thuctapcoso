using UnityEngine;
using ShootingGame.Data;
namespace ShootingGame {
    [CreateAssetMenu(fileName = "GameData", menuName = "_GAME/GameData")]
    public sealed class GameData : SingletonResourcesScriptable<GameData> {

        public Data.PlayerData Players;
        public Data.EnemyData Enemies;
        public Data.EnemyData Bosses;
        public EnemyPropertiesSO EnemyProperties;
        //public PlayerStatData PlayerStatData;
        public ItemData ItemData;

        protected override void Initialize() {
           
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("GAME/Open GameData")]
        private static void OpenGameData() {
            UnityEditor.Selection.activeObject = GameData.Instance;
        }
#endif
    }
}