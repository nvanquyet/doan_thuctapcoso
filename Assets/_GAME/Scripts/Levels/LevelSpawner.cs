using UnityEngine;
namespace ShootingGame
{
    public class LevelSpawner : Singleton<LevelSpawner>
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

        void Start() =>  _currentWave = 0;

        /// <summary>
        /// Start Wave
        /// </summary>
        private void NextWave() {
            _currentWave++;
            if(wave == null) return;
            wave.Init(GetScalingFactor(), _currentWave);
        }

        private float GetScalingFactor() => Mathf.Pow(scalingFactor, _currentWave);

        /// <summary>
        /// Call Event Start Wave
        /// </summary>
        public void OnStartWave() => NextWave();

    }

}