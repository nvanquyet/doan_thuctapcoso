using System.Collections;
using UnityEngine;
namespace ShootingGame
{

    public class LevelSpawner : Singleton<LevelSpawner>, ICoroutineBehaviour
    {
        private Coroutine spawnRoutine;

#region  Implement
        public Coroutine StartRoutine(IEnumerator routine) => StartCoroutine(routine);

        public void StopRoutine(Coroutine coroutine)
        {
            if (coroutine != null) StopCoroutine(coroutine);
        }
#endregion

        public void StartSpawning() =>  spawnRoutine = StartRoutine(Spawn());

        public void StopSpawning() => StopRoutine(spawnRoutine);

        private IEnumerator Spawn()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                Debug.Log("Spawning");
            }
        }
    }

}