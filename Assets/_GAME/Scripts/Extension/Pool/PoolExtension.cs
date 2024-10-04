
using UnityEngine;
using VawnWuyest;

public static class PoolExtensions
{
    //
    // Summary:
    //     To check if gameobject has been registered with pool, no need to register again.
    public static bool IsRegistered(this GameObject prefab)
    {
        return SingletonBehaviour<PoolManager>.Instance.IsRegistered(prefab.gameObject);
    }

    //
    // Summary:
    //     To check if gameobject can be spawn from pool, no need to instantiate new gameobject.
    public static bool HasPooled(this GameObject obj)
    {
        return SingletonBehaviour<PoolManager>.Instance.HasPooled(obj);
    }

    //
    // Summary:
    //     To check if gameobject has been spawned (currently active in scene)
    public static bool HasSpawned(this GameObject obj)
    {
        return SingletonBehaviour<PoolManager>.Instance.HasSpawned(obj);
    }

    //
    // Summary:
    //     Call this to register gameobject with Pool, so the game object will be add to
    //     pooled when recycle, else it will be destroy.
    //
    //     If your scrip inhenrit from interface "IPoolable", you no longer need to call
    //     this method.
    public static void RegisterPool<T>(this T prefab) where T : Component
    {
        SingletonBehaviour<PoolManager>.Instance.RegisterPool(prefab.gameObject, 0);
    }

    //
    // Summary:
    //     Call this to register gameobject with Pool, so the game object will be add to
    //     pooled when recycle, else it will be destroy.
    //
    //     If your scrip inhenrit from interface "IPoolable", you no longer need to call
    //     this method.
    public static void RegisterPool<T>(this T prefab, int initialPoolSize) where T : Component
    {
        SingletonBehaviour<PoolManager>.Instance.RegisterPool(prefab.gameObject, initialPoolSize);
    }

    //
    // Summary:
    //     Call this to register gameobject with Pool, so the game object will be add to
    //     pooled when recycle, else it will be destroy.
    //
    //     If your scrip inhenrit from interface "IPoolable", you no longer need to call
    //     this method.
    public static void RegisterPool(this GameObject prefab)
    {
        SingletonBehaviour<PoolManager>.Instance.RegisterPool(prefab, 0);
    }

    //
    // Summary:
    //     Call this to register gameobject with Pool, so the game object will be add to
    //     pooled when recycle, else it will be destroy.
    //
    //     If your scrip inhenrit from interface "IPoolable", you no longer need to call
    //     this method.
    public static void RegisterPool(this GameObject prefab, int initialPoolSize)
    {
        SingletonBehaviour<PoolManager>.Instance.RegisterPool(prefab, initialPoolSize);
    }

    //
    // Summary:
    //     Destroy gameobject which is register with pool (include pooled and spawned)
    public static void UnRegisterPool(this GameObject prefab)
    {
        SingletonBehaviour<PoolManager>.Instance.UnRegisterPool(prefab);
    }

    //
    // Summary:
    //     Destroy gameobject which is register with pool (include pooled and spawned)
    public static void UnRegisterPool<T>(this T prefab) where T : Component
    {
        SingletonBehaviour<PoolManager>.Instance.UnRegisterPool(prefab.gameObject);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     Source object is "IPoolable"
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    // Parameters:
    //   parent:
    //     default value = null
    //
    //   position:
    //     defaul value = prefab.position
    //
    //   scale:
    //     defaul value = prefab.localScale
    //
    //   rotation:
    //     default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Vector3 scale, Quaternion rotation) where T : Component, IPoolable
    {
        return SingletonBehaviour<PoolManager>.Instance.Spawn(prefab, parent, position, scale, rotation);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     Source object is "IPoolable"
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Vector3 scale) where T : Component, IPoolable
    {
        return prefab.Spawn(parent, position, scale, prefab.transform.rotation);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     Source object is "IPoolable"
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component, IPoolable
    {
        return prefab.Spawn(parent, position, prefab.transform.localScale, prefab.transform.rotation);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     Source object is "IPoolable"
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Transform parent) where T : Component, IPoolable
    {
        return prefab.Spawn(parent, prefab.transform.position, prefab.transform.localScale, prefab.transform.rotation);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     Source object is "IPoolable"
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab) where T : Component, IPoolable
    {
        return prefab.Spawn(null, prefab.transform.position, prefab.transform.localScale, prefab.transform.rotation);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     Source object is "IPoolable"
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component, IPoolable
    {
        return prefab.Spawn(parent, position, prefab.transform.localScale, rotation);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     Source object is "IPoolable"
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Transform parent, Quaternion rotation) where T : Component, IPoolable
    {
        return prefab.Spawn(parent, prefab.transform.position, prefab.transform.localScale, rotation);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     Source object is "IPoolable"
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Vector3 position, Vector3 scale, Quaternion rotation) where T : Component, IPoolable
    {
        return prefab.Spawn(null, position, scale, rotation);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     Source object is "IPoolable"
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Vector3 position, Vector3 scale) where T : Component, IPoolable
    {
        return prefab.Spawn(null, position, scale, prefab.transform.rotation);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     Source object is "IPoolable"
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component, IPoolable
    {
        return prefab.Spawn(null, position, prefab.transform.localScale, rotation);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     Source object is "IPoolable"
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Vector3 position) where T : Component, IPoolable
    {
        return prefab.Spawn(null, position, prefab.transform.localScale, prefab.transform.rotation);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     Source object is "IPoolable"
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Quaternion rotation) where T : Component, IPoolable
    {
        return prefab.Spawn(null, prefab.transform.position, prefab.transform.localScale, rotation);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Vector3 scale, Quaternion rotation, bool registerPoolIfNeed = true) where T : Component
    {
        return SingletonBehaviour<PoolManager>.Instance.Spawn(prefab, parent, position, scale, rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Vector3 scale, bool registerPoolIfNeed = true) where T : Component
    {
        return prefab.Spawn(parent, position, scale, prefab.transform.rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, bool registerPoolIfNeed = true) where T : Component
    {
        return prefab.Spawn(parent, position, prefab.transform.localScale, prefab.transform.rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Transform parent, bool registerPoolIfNeed = true) where T : Component
    {
        return prefab.Spawn(parent, prefab.transform.position, prefab.transform.localScale, prefab.transform.rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, bool registerPoolIfNeed = true) where T : Component
    {
        return prefab.Spawn(null, prefab.transform.position, prefab.transform.localScale, prefab.transform.rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation, bool registerPoolIfNeed = true) where T : Component
    {
        return prefab.Spawn(parent, position, Vector3.one, rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Transform parent, Quaternion rotation, bool registerPoolIfNeed = true) where T : Component
    {
        return prefab.Spawn(parent, Vector3.zero, Vector3.one, rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Vector3 position, Vector3 scale, Quaternion rotation, bool registerPoolIfNeed = true) where T : Component
    {
        return prefab.Spawn(null, position, scale, rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Vector3 position, Vector3 scale, bool registerPoolIfNeed = true) where T : Component
    {
        return prefab.Spawn(null, position, scale, Quaternion.identity, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation, bool registerPoolIfNeed = true) where T : Component
    {
        return prefab.Spawn(null, position, Vector3.one, rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Vector3 position, bool registerPoolIfNeed = true) where T : Component
    {
        return prefab.Spawn(null, position, prefab.transform.localScale, prefab.transform.rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static T Spawn<T>(this T prefab, Quaternion rotation, bool registerPoolIfNeed = true) where T : Component
    {
        return prefab.Spawn(null, Vector3.zero, Vector3.one, rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Vector3 scale, Quaternion rotation, bool registerPoolIfNeed = true)
    {
        return SingletonBehaviour<PoolManager>.Instance.Spawn(prefab, parent, position, scale, rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Vector3 scale, bool registerPoolIfNeed = true)
    {
        return prefab.Spawn(parent, position, scale, prefab.transform.rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, bool registerPoolIfNeed = true)
    {
        return prefab.Spawn(parent, position, prefab.transform.localScale, prefab.transform.rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static GameObject Spawn(this GameObject prefab, Transform parent, bool registerPoolIfNeed = true)
    {
        return prefab.Spawn(parent, prefab.transform.position, prefab.transform.localScale, prefab.transform.rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static GameObject Spawn(this GameObject prefab, bool registerPoolIfNeed = true)
    {
        return prefab.Spawn(null, prefab.transform.position, prefab.transform.localScale, prefab.transform.rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation, bool registerPoolIfNeed = true)
    {
        return prefab.Spawn(parent, position, prefab.transform.localScale, rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static GameObject Spawn(this GameObject prefab, Transform parent, Quaternion rotation, bool registerPoolIfNeed = true)
    {
        return prefab.Spawn(parent, prefab.transform.position, prefab.transform.localScale, rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static GameObject Spawn(this GameObject prefab, Vector3 position, Vector3 scale, Quaternion rotation, bool registerPoolIfNeed = true)
    {
        return prefab.Spawn(null, position, scale, rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static GameObject Spawn(this GameObject prefab, Vector3 position, Vector3 scale, bool registerPoolIfNeed = true)
    {
        return prefab.Spawn(null, position, scale, prefab.transform.rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation, bool registerPoolIfNeed = true)
    {
        return prefab.Spawn(null, position, prefab.transform.localScale, rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static GameObject Spawn(this GameObject prefab, Vector3 position, bool registerPoolIfNeed = true)
    {
        return prefab.Spawn(null, position, prefab.transform.localScale, prefab.transform.rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Instantiate (clone) a game object from "prefab" object. New spawned object aready
    //     set active true.
    //
    //     If "prefab" is inherit from interface "IPoolable", or was register by "CreatePool"
    //     before, output gameObject will be add into pooled list and can be reuse when
    //     recycle.
    //
    //     - *position*: defaul value = prefab.position
    //
    //     - *scale*: defaul value = prefab.localScale
    //
    //     - *rotation*: default value = prefab.rotation
    public static GameObject Spawn(this GameObject prefab, Quaternion rotation, bool registerPoolIfNeed = true)
    {
        return prefab.Spawn(null, prefab.transform.position, prefab.transform.localScale, rotation, registerPoolIfNeed);
    }

    //
    // Summary:
    //     Remove game object from the scene (Disable or Destroy)
    //
    //     If game object is "IPoolable" (or was register by "RegisterPool"), it will be
    //     set to DISABLE, else it will be DESTROY
    public static void Recycle<T>(this T obj) where T : Component
    {
        SingletonBehaviour<PoolManager>.Instance.Recycle(obj);
    }

    //
    // Summary:
    //     Remove game object from the scene (Disable or Destroy)
    //
    //     If game object is "IPoolable" (or was register by "RegisterPool"), it will be
    //     set to DISABLE, else it will be DESTROY
    public static void Recycle(this GameObject obj)
    {
        SingletonBehaviour<PoolManager>.Instance.Recycle(obj);
    }

    //
    // Summary:
    //     Remove game object from the scene (Disable or Destroy)
    //
    //     If game object is "IPoolable" (or was register by "RegisterPool"), it will be
    //     set to DISABLE, else it will be DESTROY
    public static void RecycleAll<T>(this T prefab) where T : Component
    {
        SingletonBehaviour<PoolManager>.Instance.RecycleAll(prefab);
    }

    //
    // Summary:
    //     Remove game object from the scene (Disable or Destroy)
    //
    //     If game object is "IPoolable" (or was register by "RegisterPool"), it will be
    //     set to DISABLE, else it will be DESTROY
    public static void RecycleAll(this GameObject prefab)
    {
        SingletonBehaviour<PoolManager>.Instance.RecycleAll(prefab);
    }
}