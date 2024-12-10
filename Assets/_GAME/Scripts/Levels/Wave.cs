using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ShootingGame
{
    /// <summary>
    /// Wave Properties to set data wave
    /// </summary>
    [System.Serializable]
    public struct WaveProperties
    {
        public float timeNormalSpawn;
        public float timeThreshold;
        public int spawnThreshold;
        public int strengthWave;
        public int timeWave;
    }

    public class Wave : MonoBehaviour, ICoroutineBehaviour
    {

        [Header("All Position Spawn")]
        [SerializeField] private List<Transform> spawnPositions = new();
        [SerializeField] private Timer timmer;

        [Header("Config")]
        [SerializeField] private float spawnRadius = 10f;
        [SerializeField] private float timeDelaySpawn = 2f;


        //This properties is base data of first wave

        public bool IsWaveClear => !isSpawning && enemies.Count <= 0;

        private Coroutine spawnRoutine;
        //This properties to spawn and set data enemy=
        private int currentWave;

        //This properties is data of wave
        private bool isBossWave;
        private WaveProperties waveProperties;
        //This properties is flag to check spawn done
        private bool isSpawning;
        private List<Enemy> enemies = new();
        private List<Transform> tsEnemies = new();

        public List<Transform> TransformEnemies => tsEnemies;

        protected virtual void OnValidate()
        {
            timmer = FindObjectOfType<Timer>();
            spawnPositions = GetComponentsInChildren<Transform>().ToList();
            spawnPositions.RemoveAt(0);
            foreach (Transform spawn in spawnPositions)
            {
                // Generate a random point within the radius
                Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;

                // Apply the random position to the spawn point (keeping original z-axis)
                spawn.position = new Vector3(randomPoint.x, randomPoint.y, spawn.position.z);
            }
        }

        //This method to set data wave
        public void Init(float scalingFactor, int currentWave)
        {
            this.isBossWave = currentWave % GameConfig.Instance.bossWaveDistance == 0;

            this.currentWave = currentWave;

            this.waveProperties = GameService.CalculateWaveProperties(currentWave, scalingFactor);

            timmer.SetTimer(this.waveProperties.timeWave, () =>
            {
                GameCtrl.Instance.OnEndGame(false, 0);
            });
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
        public void StartSpawning()
        {
            spawnRoutine = StartRoutine(IESpawn());
        }
        public void StopSpawning() => StopRoutine(spawnRoutine);

        /// <summary>
        /// Spawn Enemy Coroutine
        /// </summary>
        /// <returns></returns>
        private IEnumerator IESpawn()
        {
            isSpawning = true;
            GameService.ClearList(ref enemies);
            GameService.ClearList(ref tsEnemies);

            var allEnimies = GameData.Instance.Enemies.GetAllValue();
            var curEnemyCount = 0;
            GameService.LogColor($"Strength Wave: {waveProperties.strengthWave}");
            while (waveProperties.strengthWave > 0)
            {
                //Spawn Enemy from data
                var enemy = allEnimies[Random.Range(0, Mathf.Min(currentWave, allEnimies.Length))];
                //Init data enemy
                if (enemy != null)
                {
                    var enemyInstance = Instantiate(enemy, spawnPositions[Random.Range(0, spawnPositions.Count)]);
                    //Init data enemy
                    if (enemyInstance)
                    {
                        enemyInstance.transform.localPosition = Vector3.zero;
                        enemyInstance.Init(currentWave);
                        AddEnemy(enemyInstance);

                        waveProperties.strengthWave -= enemyInstance.GetStrength();
                        GameCtrl.Instance.EnemySpawned();
                        curEnemyCount++;
                    }
                    yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.3f));
                }

                if (curEnemyCount % waveProperties.spawnThreshold == 0)
                {
                    yield return new WaitForSeconds(waveProperties.timeThreshold);
                }
                else
                {
                    yield return new WaitForSeconds(waveProperties.timeNormalSpawn);
                }

            }

            if (isBossWave)
            {
                //Spawn Boss
                SpawnBoss();
            }

            isSpawning = false;
            yield break;
        }

        private void SpawnBoss()
        {
            //Spawn Boss from data
            var boss = GameData.Instance.Bosses.GetAllValue()[Mathf.Max((currentWave / GameConfig.Instance.bossWaveDistance) - 1, 0)];
            if (boss != null)
            {
                var bossInstance = Instantiate(boss, spawnPositions[Random.Range(0, spawnPositions.Count)]);
                //Init data enemy
                if (bossInstance)
                {
                    bossInstance.transform.localPosition = Vector3.zero;
                    bossInstance.Init(currentWave);
                    AddEnemy(bossInstance);
                }
            }
        }

        public bool OnEnemyDeath(Enemy enemy)
        {
            if (enemy == null || !enemies.Contains(enemy)) return false;
            enemies.Remove(enemy);
            if (tsEnemies.Contains(enemy.transform)) tsEnemies.Remove(enemy.transform);
            return true;
        }



        private void AddEnemy(Enemy target)
        {
            if (!enemies.Contains(target)) enemies.Add(target);
            if (!tsEnemies.Contains(target.transform)) tsEnemies.Add(target.transform);
        }


    }

}