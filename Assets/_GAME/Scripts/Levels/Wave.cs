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

        public Timer Timmer => timmer;
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
        public List<Enemy> enemies { get; private set; } = new();
        public List<Transform> tsEnemies { get; private set; } = new();

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
            this.isBossWave = currentWave % GameConfig.Instance.BossWaveDistance == 0;
            this.currentWave = currentWave;

            this.waveProperties = GameService.CalculateWaveProperties(currentWave, scalingFactor);
            
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
            UICtrl.Instance.Get<InGameUI>().SetWaveText(currentWave);

            timmer.SetTimer(this.waveProperties.timeWave, () =>
            {
                GameCtrl.Instance.OnEndGame(false, 0);
            });

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
            if(enemies != null && enemies.Count > 0) { enemies.Clear(); }
            if(tsEnemies != null && tsEnemies.Count > 0) { tsEnemies.Clear(); }
            var allEnimies = GameData.Instance.Enemies.GetAllValue();
            var curEnemyCount = 0;

            while (waveProperties.strengthWave > 0)
            {
                //Spawn Enemy from data
                var enemyData = allEnimies[Random.Range(0, Mathf.Min(currentWave, allEnimies.Length) - 1)];
                //Init data enemy
                if (enemyData != null && enemyData.Prefabs != null)
                {
                    var enemyInstance = Instantiate(enemyData.Prefabs, spawnPositions[Random.Range(0, spawnPositions.Count)]);
                    //Init data enemy
                    if (enemyInstance)
                    {
                        enemyInstance.transform.localPosition = Vector3.zero;
                        enemyInstance.Init(enemyData, currentWave, false);
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

            if (isBossWave) SpawnBoss();

            isSpawning = false;
            yield break;
        }

        private void SpawnBoss()
        {
            //Spawn Boss from data
            var index = Mathf.Max((currentWave / GameConfig.Instance.BossWaveDistance) - 1, 0);
            GameService.LogColor($"Spawn Boss {index}");
            var bossData = GameData.Instance.Bosses.GetValue(index);
            if (bossData && bossData.Prefabs)
            {
                var bossInstance = Instantiate(bossData.Prefabs, spawnPositions[Random.Range(0, spawnPositions.Count)]);
                //Init data enemy
                if (bossInstance)
                {
                    UICtrl.Instance.Get<InGameUI>().ActiveBossProgess(true, true);
                    bossInstance.transform.position = Vector3.zero;
                    bossInstance.Init(bossData, currentWave, true, UICtrl.Instance.Get<InGameUI>().SetIconBoss);
                    bossInstance.OnDeadAction += (_) =>
                    {
                        UICtrl.Instance.Get<InGameUI>().ActiveBossProgess(false);
                    };
                    bossInstance.OnDefendAction += UICtrl.Instance.Get<InGameUI>().SetBossProgess;
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