using UnityEngine;
namespace VawnWuyest {
    [CreateAssetMenu(fileName = "GameData", menuName = "_GAME/GameData")]
    public sealed class GameData : SingletonResourcesScriptable<GameData> {

        public Data.PlayerData Players;
        public Data.EnemyData Enemies;

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