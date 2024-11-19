using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ShootingGame
{
    public class ObjectPooling<T> where T : Component
    {
        private Queue<T> _objects = new Queue<T>();
        private T _prefab;
        private Transform _parentTransform;

        public ObjectPooling(T prefab, int initialSize, Transform parentTransform = null)
        {
            _prefab = prefab;
            _parentTransform = parentTransform;
            for (int i = 0; i < initialSize; i++)
            {
                T newObject = GameObject.Instantiate(_prefab, _parentTransform);
                newObject.gameObject.SetActive(false);
                _objects.Enqueue(newObject);
            }
        }

        public T Get()
        {
            
            if (_objects.Count <= 0)
            {
                T newObject = Object.Instantiate(_prefab, _parentTransform);
                newObject.gameObject.SetActive(false);
                _objects.Enqueue(newObject);
            }
            T objectToGet = _objects.Dequeue();
            if (objectToGet && !objectToGet.gameObject.activeInHierarchy) objectToGet.gameObject.SetActive(true);
            return objectToGet;
        }

        public void Recycle(T objectToReturn)
        {
            if(objectToReturn != null && objectToReturn.gameObject.activeInHierarchy) objectToReturn?.gameObject.SetActive(false);
            _objects.Enqueue(objectToReturn);
        }
    }
}