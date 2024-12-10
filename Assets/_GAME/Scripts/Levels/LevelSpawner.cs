using System;
using System.Collections.Generic;
using UnityEngine;
namespace ShootingGame
{
    public class LevelSpawner : ShootingGame.SingletonBehaviour<LevelSpawner>
    {
        /// <summary>
        /// Scaling Factor
        /// </summary>
        [SerializeField] private float scalingFactor = 1.1f;

        /// <summary>
        /// Wave Prefab
        /// </summary>
        [SerializeField] private Wave wave;

        private int _currentWave;

        protected virtual void OnValidate() => wave = GetComponentInChildren<Wave>();

        void Start() {
            _currentWave = 0;
        }

        /// <summary>
        /// Start Wave
        /// </summary>
        public void NextWave() {
            _currentWave++;
            OnStartWave();
        }

        /// <summary>
        /// Call Event Start Wave
        /// </summary>
        public void OnStartWave() {
            if(wave == null) return;
            wave.Init(GameConfig.Instance.scalingFactor, _currentWave);
        }

        public List<Transform> GetActiveEnemies()
        {
            if(wave == null) return null;
            return wave.TransformEnemies;
        }

        public bool OnEnemyDeath(Enemy enemy)
        {
            var isDeath = wave.OnEnemyDeath(enemy);

            GameCtrl.Instance.OnEnemyDeath();

            return isDeath;
        }

        internal bool IsWaveClear
        {
            get
            {
                if (wave == null) return false;
                return wave.IsWaveClear;
            }
        }
    }

}