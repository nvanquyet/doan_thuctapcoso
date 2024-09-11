using System.Collections;
using UnityEngine;
using VawnWuyest;
namespace ShootingGame
{
    /// <summary>
    /// Wave Properties to set data wave
    /// </summary>
    [System.Serializable]
    public struct WaveProperties
    {
        public int enemyCount;
        public int timeWave;
        public float timeBtwSpawn;
        public float timeThreshold;
        public int spawnThreshold;
    }

    public class Wave : MonoBehaviour, ICoroutineBehaviour
    {
        //This properties is base data of first wave
        [SerializeField] private WaveProperties baseWaveProperties;

        //This properties is distance wave to spawn boss
        [SerializeField] private int bossWaveDistance = 5;
        private Coroutine spawnRoutine;
        //This properties to spawn and set data enemy
        private float scalingFactor;
        private int currentWave;

        //This properties is data of wave
        private bool isBossWave;
        private WaveProperties waveProperties;
        //This properties is flag to check spawn done
        public bool IsSpawnDone {
            get;
            private set;
        } 

        //This method to set data wave
        public void Init(float scalingFactor, int currentWave)
        {
            this.scalingFactor = scalingFactor;
            this.isBossWave = currentWave % bossWaveDistance == 0;

            this.waveProperties = new WaveProperties
            {
                enemyCount = Mathf.RoundToInt(baseWaveProperties.enemyCount * Mathf.Pow(scalingFactor, currentWave)),
                timeWave = Mathf.RoundToInt(baseWaveProperties.timeWave * Mathf.Pow(scalingFactor, currentWave)),
                timeThreshold = Mathf.RoundToInt(baseWaveProperties.timeThreshold * Mathf.Pow(scalingFactor, currentWave)),                
                spawnThreshold = Mathf.RoundToInt(baseWaveProperties.spawnThreshold * Mathf.Pow(scalingFactor, currentWave)),
            };

            StartSpawning();
        }

#region  Implement
        public Coroutine StartRoutine(IEnumerator routine) => StartCoroutine(routine);

        public void StopRoutine(Coroutine coroutine)
        {
            if (coroutine != null) StopCoroutine(coroutine);
        }
#endregion
        /// <summary>
        /// Start Spawning
        /// </summary>
        public void StartSpawning() {
            IsSpawnDone = false;
            spawnRoutine = StartRoutine(Spawn());
        }
        public void StopSpawning() => StopRoutine(spawnRoutine);

        /// <summary>
        /// Spawn Enemy Coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator Spawn()
        {
            var curEnemyCount = 0;
            while (true)
            {
                //Spawn Enemy from data

                curEnemyCount++;
                if(curEnemyCount >= waveProperties.enemyCount){
                    break;
                } else if (curEnemyCount % waveProperties.spawnThreshold == 0)
                {
                    yield return new WaitForSeconds(waveProperties.timeThreshold);
                }else {
                    yield return new WaitForSeconds(waveProperties.timeBtwSpawn);
                }
            }

            if(isBossWave){
                //Spawn Boss
            }
        }

    }

}