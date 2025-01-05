using System;
using System.Collections.Generic;
using UnityEngine;
namespace ShootingGame
{
    public class LevelSpawner : ShootingGame.SingletonBehaviour<LevelSpawner>
    {
        /// <summary>
        /// Wave Prefab
        /// </summary>
        [SerializeField] private Wave wave;

        public Wave Wave => wave;

        private int _currentWave;

        protected virtual void OnValidate() => wave = GetComponentInChildren<Wave>();

        void Start() => _currentWave = 0;

        /// <summary>
        /// Start Wave
        /// </summary>
        public void NextWave()
        {
            _currentWave++;
            OnStartWave();
        }

        /// <summary>
        /// Call Event Start Wave
        /// </summary>
        public void OnStartWave() => wave?.Init(GameConfig.Instance.ScalingFactor, _currentWave);

        public List<Transform> GetActiveEnemies() => wave?.tsEnemies;

        public bool OnEnemyDeath(Enemy enemy)
        {
            var isDeath = wave.OnEnemyDeath(enemy);
            GameCtrl.Instance.OnEnemyDeath();
            return isDeath;
        }

        internal void OnEndGame()
        {
            if (wave == null || wave.enemies == null || wave.enemies.Count <= 0) return;
            for (int i = 0; i < wave.enemies.Count; i++)
            {
                if (wave.enemies[i] != null)
                {
                    wave.enemies[i].ForceDead();
                }
            }
            wave.enemies.Clear();
            wave.tsEnemies.Clear();
        }

        internal bool IsWaveClear =>  wave? wave.IsWaveClear : false;
    }

}