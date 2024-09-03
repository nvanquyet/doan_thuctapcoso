using UnityEngine;
namespace ShootingGame
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        protected virtual bool GetDontDestroyOnLoad()
        {
            return false;
        }
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        instance = obj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                if (GetDontDestroyOnLoad())
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
            OnAwake();
        }
        
        protected virtual void OnAwake(){

        }
    }

    public abstract class SingletonDontDestroy<T> : Singleton<T> where T : MonoBehaviour
    {
        protected override bool GetDontDestroyOnLoad()
        {
            return true;
        }
    }
}