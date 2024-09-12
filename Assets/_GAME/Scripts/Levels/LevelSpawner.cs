using System;
using UnityEngine;
namespace ShootingGame
{
    public class LevelSpawner : VawnWuyest.Singleton<LevelSpawner>
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
            OnStartWave();
        }

        /// <summary>
        /// Start Wave
        /// </summary>
        public void NextWave() {
            _currentWave++;
            OnStartWave();
        }

        private float GetScalingFactor() => Mathf.Pow(scalingFactor, _currentWave);

        /// <summary>
        /// Call Event Start Wave
        /// </summary>
        public void OnStartWave() {
            if(wave == null) return;
            wave.Init(GetScalingFactor(), _currentWave);
        }


        public bool OnEnemyDeath(Enemy enemy)
        {
            var isDeath = wave.OnEnemyDeath(enemy);

            GameCtrl.Instance.OnCheckWaveClear();

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