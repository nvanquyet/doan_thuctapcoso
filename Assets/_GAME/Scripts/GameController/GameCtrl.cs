
using System.Collections.Generic;
using UnityEngine;
using ShootingGame;
namespace ShootingGame
{
    public class GameCtrl : ShootingGame.SingletonBehaviour<GameCtrl>
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
        public Player GetRandomPlayer()
        {
            if (_players == null || _players.Count <= 0) return null;
            return _players[Random.Range(0, _players.Count)];
        }

        /// <summary>
        /// Start Game
        /// </summary>
        public void OnStartGame() => this.Dispatch<GameEvent.OnWaveClear>(); 

        /// <summary>
        /// Show Merge and Upgrade UI
        /// </summary>
        private void OnWaveClear() => this.Dispatch<GameEvent.OnWaveClear>(); 

        internal void OnCheckWaveClear()
        {
            if(LevelSpawner.Instance.IsWaveClear){
                Invoke(nameof(OnWaveClear), 1f);
            }
        }
        internal void NextWave()
        {
            LevelSpawner.Instance.NextWave();
        }

        internal void SpawnItem()
        {
            Debug.Log("Spawn Item");
        }
    }

}