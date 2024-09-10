using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ShootingGame
{
    public partial interface Interface
    {
        public interface ISpawner
        { 
            void Spawn();
        }

        public interface IMoveable
        {
            
            void Move(Vector3 direction);
        }
    }


    public interface ICoroutineBehaviour
    {
        Coroutine StartRoutine(IEnumerator routine);
        void StopRoutine(Coroutine coroutine);
    }
}

