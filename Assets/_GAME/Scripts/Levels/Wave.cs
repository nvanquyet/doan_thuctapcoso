using Mono.CSharp;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [Header("All Position Spawn")]
        [SerializeField] private List<Transform> spawnPositions = new();

        [Header("Config")]
        [SerializeField] private float spawnRadius = 10f;
        [SerializeField] private float timeDelaySpawn = 2f;
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


        protected virtual void OnValidate(){
            spawnPositions = GetComponentsInChildren<Transform>().ToList();
            spawnPositions.RemoveAt(0);
            foreach (Transform spawn in spawnPositions)
            {
                // Generate a random point within the radius
                Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;

                // Apply the random position to the spawn point (keeping original z-axis)
                spawn.position = new Vector3(spawn.position.x + randomPoint.x, spawn.position.y + randomPoint.y, spawn.position.z);
            }
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

            Invoke(nameof(StartSpawning), timeDelaySpawn);
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
            var allEnimies = GameData.Instance.Enemies.GetAllValue();
            while (true)
            {
                //Spawn Enemy from data
                var enemy = allEnimies[Random.Range(0, Mathf.Min(currentWave, allEnimies.Length))];
                //Init data enemy
                if(enemy != null) {
                    var randomPos = spawnPositions[Random.Range(0, spawnPositions.Count)];
                    var enemyInstance = Instantiate(enemy, randomPos);
                    Debug.Log("Spawn Enemy");
                    //Init data enemy
                    if(enemyInstance) {
                        enemyInstance.transform.localPosition = Vector3.zero;
                        enemyInstance.Init(scalingFactor);
                        curEnemyCount++;
                    }
                }
                if(curEnemyCount >= waveProperties.enemyCount){
                    yield break;
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