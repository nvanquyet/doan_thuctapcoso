
using System.Collections.Generic;
using UnityEngine;
namespace ShootingGame
{
    public class GameCtrl : VawnWuyest.Singleton<GameCtrl>
    {
        private List<Player> _players;

        /// <summary>
        /// Add Player
        /// </summary>
        public void AddPlayer(Player player)
        {
            if (player == null) return;
            if (_players == null) _players = new List<Player>();
            if (!_players.Contains(player)) _players.Add(player);
        }
        
        /// <summary>
        /// Remove Player
        /// </summary>
        /// <param name="player"></param>
        public void RemovePlayer(Player player) {
            if (player == null || _players == null) return;
            if (_players.Contains(player)) _players.Remove(player);
        }

        /// <summary>
        /// Get Random Player
        /// </summary>
        /// <returns></returns>
        public Transform GetRandomTransformPlayer()
        {
            if (_players == null || _players.Count <= 0) return null;
            return _players[Random.Range(0, _players.Count)].transform;
        }

        /// <summary>
        /// Start Game
        /// </summary>
        public void OnStartGame() => LevelSpawner.Instance.OnStartWave();

        /// <summary>
        /// Show Merge and Upgrade UI
        /// </summary>
        private void OnWaveClear() => LevelSpawner.Instance.OnStartWave();

        internal void OnCheckWaveClear()
        {
            Debug.Log($"Wave Clear {LevelSpawner.Instance.IsWaveClear}");
            //if(LevelSpawner.Instance.IsWaveClear) OnWaveClear();
        }
    }

}