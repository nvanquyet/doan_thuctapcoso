
using System.Collections.Generic;
using UnityEngine;
namespace ShootingGame
{
    public class GameCtrl : Singleton<GameCtrl>
    {
        private List<Player> _players;

        public void AddPlayer(Player player)
        {
            if (player == null) return;
            if (_players == null) _players = new List<Player>();
            if (!_players.Contains(player)) _players.Add(player);
        }

        public void RemovePlayer(Player player) {
            if (player == null || _players == null) return;
            if (_players.Contains(player)) _players.Remove(player);
        }

        public Transform GetRandomTransformPlayer()
        {
            if (_players == null || _players.Count <= 0) return null;
            return _players[Random.Range(0, _players.Count)].transform;
        }
    }

}