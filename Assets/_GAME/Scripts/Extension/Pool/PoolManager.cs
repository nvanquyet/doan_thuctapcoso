
using System.Collections.Generic;
using UnityEngine;
using ShootingGame;
public interface IPoolable
{
    //
    // Summary:
    //     This will be call automaticaly by "PoolManager.Spawn". Do not call this outside.
    //
    //
    //     Init your variable inside here.
    void OnSpawnCallback();

    //
    // Summary:
    //     This will be call automaticaly by "PoolManager.Recycle". Do not call this outside.
    //
    //
    //     Put your callback action inside here. This call before OnDisable
    void OnRecycleCallback();
}

public sealed class PoolManager : SingletonBehaviour<PoolManager>
{
    private Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();

    private Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();

    protected override void OnAwake()
    {
    }

    //
    // Summary:
    //     Call this to register gameobject with Pool, so the game object will be add to
    //     pooled when recycle, else it will be destroy.
    //
    //     If your scrip inhenrit from interface "IPoolable", you no longer need to call
    //     this method.
    public void RegisterPool(GameObject prefab, int initialPoolSize)
    {
        if (prefab == null)
        {
            return;
        }

        List<GameObject> value = null;
        if (SingletonBehaviour<PoolManager>.Instance.pooledObjects.ContainsKey(prefab))
        {
            SingletonBehaviour<PoolManager>.Instance.pooledObjects.TryGetValue(prefab, out value);
            if (value == null)
            {
                value = new List<GameObject>();
                SingletonBehaviour<PoolManager>.Instance.pooledObjects[prefab] = value;
            }
        }
        else
        {
            value = new List<GameObject>();
            SingletonBehaviour<PoolManager>.Instance.pooledObjects.Add(prefab, value);
        }

        if (initialPoolSize > value.Count)
        {
            while (value.Count < initialPoolSize)
            {
                GameObject gameObject = Object.Instantiate(prefab, base.transform);
                gameObject.SetActive(value: false);
                value.Add(gameObject);
            }
        }
    }

    public T Spawn<T>(T prefab, Transform parent, Vector3 position, Vector3 scale, Quaternion rotation) where T : Component, IPoolable
    {
        return Spawn(prefab, parent, position, scale, rotation, createPoolIfNeed: true);
    }

    public T Spawn<T>(T prefab, Transform parent, Vector3 position, Vector3 scale, Quaternion rotation, bool createPoolIfNeed) where T : Component
    {
        if ((Object)prefab == (Object)null || prefab.gameObject == null)
        {
            Debug.LogError("[PoolManager] Cannot spawn a null object. Return value=null");
            return null;
        }

        return Spawn(prefab.gameObject, parent, position, scale, rotation, createPoolIfNeed).GetComponent<T>();
    }

    public GameObject Spawn(GameObject prefab, Transform parent)
    {
        return Spawn(prefab, parent, prefab.transform.position, prefab.transform.localScale, prefab.transform.rotation, createPoolIfNeed: true);
    }

    public GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Vector3 scale, Quaternion rotation, bool createPoolIfNeed)
    {
        if (prefab == null)
        {
            Debug.LogError("[PoolManager] Cannot spawn a null prefab!");
            return null;
        }

        if (!createPoolIfNeed)
        {
            createPoolIfNeed = prefab.GetComponent<IPoolable>() != null;
        }

        GameObject gameObject = null;
        if (SingletonBehaviour<PoolManager>.Instance.pooledObjects.TryGetValue(prefab, out var value))
        {
            if (value.Count > 0)
            {
                while (gameObject == null && value.Count > 0)
                {
                    gameObject = value[0];
                    value.RemoveAt(0);
                }

                _ = gameObject != null;
                return SingletonBehaviour<PoolManager>.Instance.GetObject(gameObject, prefab, parent, position, scale, rotation);
            }

            return SingletonBehaviour<PoolManager>.Instance.GetObject(gameObject, prefab, parent, position, scale, rotation);
        }

        return SingletonBehaviour<PoolManager>.Instance.GetObject(gameObject, prefab, parent, position, scale, rotation, createPoolIfNeed, createPoolIfNeed);
    }

    private GameObject GetObject(GameObject obj, GameObject prefab, Transform parent, Vector3 position, Vector3 scale, Quaternion rotation, bool addToSpawns = true, bool createPool = false)
    {
        if (obj == null)
        {
            obj = Object.Instantiate(prefab, position, rotation, parent);
        }

        obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.localScale = scale;
        obj.transform.rotation = rotation;
        obj.SetActive(value: true);
        if (createPool)
        {
            RegisterPool(obj, 0);
        }

        if (addToSpawns)
        {
            SingletonBehaviour<PoolManager>.Instance.spawnedObjects.Add(obj, prefab);
        }

        obj.GetComponent<IPoolable>()?.OnSpawnCallback();
        return obj;
    }

    public void Recycle<T>(T obj) where T : Component
    {
        if (!((Object)obj == (Object)null))
        {
            Recycle(obj.gameObject);
        }
    }

    public void Recycle(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }

        IPoolable component = obj.GetComponent<IPoolable>();
        component?.OnRecycleCallback();
        obj.transform.SetParent(SingletonBehaviour<PoolManager>.Instance.transform);
        obj.SetActive(value: false);
        if (SingletonBehaviour<PoolManager>.Instance.spawnedObjects.TryGetValue(obj, out var value))
        {
            SingletonBehaviour<PoolManager>.Instance.spawnedObjects.Remove(obj);
            if (value != null && SingletonBehaviour<PoolManager>.Instance.pooledObjects.ContainsKey(value))
            {
                SingletonBehaviour<PoolManager>.Instance.pooledObjects[value].Add(obj);
            }
            else if (value != null && component != null)
            {
                SingletonBehaviour<PoolManager>.Instance.pooledObjects.Add(value, new List<GameObject> { obj });
            }
            else
            {
                Object.Destroy(obj);
            }
        }
        else
        {
            Object.Destroy(obj);
        }
    }

    public void RecycleAll<T>(T prefab) where T : Component
    {
        if (!((Object)prefab == (Object)null))
        {
            RecycleAll(prefab.gameObject);
        }
    }

    public void RecycleAll(GameObject prefab)
    {
        if (prefab == null)
        {
            return;
        }

        List<GameObject> list = new List<GameObject>();
        foreach (KeyValuePair<GameObject, GameObject> spawnedObject in SingletonBehaviour<PoolManager>.Instance.spawnedObjects)
        {
            if (spawnedObject.Key != null && spawnedObject.Value == prefab)
            {
                list.Add(spawnedObject.Key);
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            Recycle(list[i]);
        }
    }

    public void RecycleAll()
    {
        List<GameObject> list = new List<GameObject>();
        list.AddRange(SingletonBehaviour<PoolManager>.Instance.spawnedObjects.Keys);
        for (int i = 0; i < list.Count; i++)
        {
            Recycle(list[i]);
        }
    }

    //
    // Summary:
    //     To check if gameobject has been registered with pool, no need to register again.
    public bool IsRegistered(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("[PoolManager] Cannot check 'IsPoolable' with null game object.");
            return false;
        }

        if (!HasPooled(obj) && !HasSpawned(obj))
        {
            return obj.GetComponent<IPoolable>() != null;
        }

        return true;
    }

    //
    // Summary:
    //     To check if gameobject can be spawn from pool, no need to instantiate new gameobject.
    public bool HasPooled(GameObject obj)
    {
        return SingletonBehaviour<PoolManager>.Instance.pooledObjects.ContainsKey(obj);
    }

    //
    // Summary:
    //     To check if gameobject has been spawned (currently active in scene)
    public bool HasSpawned(GameObject obj)
    {
        return SingletonBehaviour<PoolManager>.Instance.spawnedObjects.ContainsKey(obj);
    }

    //
    // Summary:
    //     Destroy gameobject which is register with pool (include pooled and spawned)
    public void UnRegisterPool(GameObject prefab)
    {
        if (!(prefab == null) && IsRegistered(prefab))
        {
            RecycleAll(prefab);
            DestroyPooled(prefab);
        }
    }

    //
    // Summary:
    //     Destroy gameobject which is register with pool (include pooled and spawned)
    public void UnRegisterPool<T>(T prefab) where T : Component
    {
        if (!((Object)prefab == (Object)null))
        {
            UnRegisterPool(prefab.gameObject);
        }
    }

    //
    // Summary:
    //     Destroy all gameobject which is currently in pooled (currently disable in scene)
    //
    //
    //     No action with spawned (currently active in scene)
    public void DestroyPooled(GameObject prefab)
    {
        if (!(prefab == null) && SingletonBehaviour<PoolManager>.Instance.pooledObjects.TryGetValue(prefab, out var value))
        {
            for (int i = 0; i < value.Count; i++)
            {
                Object.Destroy(value[i]);
            }

            value.Clear();
        }
    }

    //
    // Summary:
    //     Destroy all gameobject which is currently in pooled (currently disable in scene)
    //
    //
    //     No action with spawned (currently active in scene)
    public void DestroyPooled<T>(T prefab) where T : Component
    {
        if (!((Object)prefab == (Object)null))
        {
            DestroyPooled(prefab.gameObject);
        }
    }

    //
    // Summary:
    //     Destroy all gameobject which is managing by PoolManager, include all spawned
    //     and pooled.
    public void DestroyAll()
    {
        try
        {
            RecycleAll();
            List<GameObject> list = new List<GameObject>();
            list.AddRange(SingletonBehaviour<PoolManager>.Instance.pooledObjects.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                DestroyPooled(list[i]);
            }
        }
        catch
        {
        }
    }
}