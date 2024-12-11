
using System.Collections.Generic;
using UnityEngine;
using ShootingGame;
namespace ShootingGame
{
    public class GameCtrl : ShootingGame.SingletonBehaviour<GameCtrl>
    {
        private List<Player> _players;

        private int enemiesDefeated = 0;
        private int totalEnemies = 0;

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
        public void OnGameStart()
        {
            enemiesDefeated = 0;
            totalEnemies = 0;
        }

        /// <summary>
        /// OnEndGame
        /// </summary>
        /// <param name="isWin"></param>
        /// <param name="timeLeft"></param>
        internal void OnEndGame(bool isWin, int timeLeft)
        {
            LevelSpawner.Instance.OnEndGame();
            this.Dispatch<GameEvent.OnEndGame>(new GameEvent.OnEndGame() { enemiesDefeated = this.enemiesDefeated, timeLeft = timeLeft, totalEnemies = totalEnemies, isWin = isWin });
        }

        /// <summary>
        /// Show Merge and Upgrade UI
        /// </summary>
        private void OnWaveClear()
        {
            this.Dispatch<GameEvent.OnWaveClear>();
        }

        internal void NextWave(GameEvent.OnNextWave param)
        {
            foreach (var player in _players)
            {
                if(player.Equals(param.player)) player.OnNextWave(param);
            }
            this.Dispatch<GameEvent.OnNextWave>(param);
            LevelSpawner.Instance.NextWave();
        }

       
        internal void OnEnemyDeath()
        {
            enemiesDefeated++;
            if (LevelSpawner.Instance.IsWaveClear)
            {
                Invoke(nameof(OnWaveClear), 1f);
            }
        }


        
        internal void EnemySpawned() => totalEnemies++;

        internal void OnPlayerDead(Player player)
        {
            RemovePlayer(player);
            if (_players == null || _players.Count <= 0)
            {
                OnEndGame(false, LevelSpawner.Instance.Wave.Timmer.TimeLeft);
            }
        }
    }

}