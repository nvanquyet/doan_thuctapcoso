using UnityEngine;

namespace VawnWuyest
{

    public interface IDisposable
    {
        void Dispose();
    }
    public abstract class Service<T> : IDisposable where T : new()
    {
        //
        // Summary:
        //     This is local variable. Use 'Instance' from outsite.
        private static T __instance;

        private static bool destroying;

        //
        // Summary:
        //     If you want to check null, use this property instead of calling to "Instance"
        //     because "Instance" can auto create an emty object, so "Instance" can never null
        public static bool Initialized => __instance != null;

        //
        // Summary:
        //     This will auto create new if instance is null.
        //
        //     If you want to check null, use 'if (T.Initialized)' instead of 'if (T.Instance
        //     != null)'
        public static T Instance
        {
            get
            {
                if (__instance != null || destroying)
                {
                    return __instance;
                }

                __instance = new T();
                return __instance;
            }
        }

        //
        // Summary:
        //     Constructor will call automaticaly on the first call of "Instance"
        //
        //     If you want to create your custom constructor, note that deliver from this "base"
        protected Service()
        {
            Initialize();
            Debug.Log("[Service][" + typeof(T).Name + "] Initialized");
        }

        //
        // Summary:
        //     This will call automaticly by GC.Collect when no one reference to this instance.
        ~Service()
        {
            Dispose();
        }

        //
        // Summary:
        //     This function will be call automaticaly only one times (on the first call of
        //     "Instance")
        //
        //     Put your custom initialize here. No need to call "base.Initialize"
        protected abstract void Initialize();

        //
        // Summary:
        //     This method is empty function, just use to prepare initialize the "Instance"
        //     to improve Ram/ CPU (to preload or decompress all asset inside)
        //
        //     Call this at the first application script (exp: BasePreload.cs)
        //
        //     Be carefully if you override this and put your custom initialzation here, because
        //     this function can be call many times on any where, so the initialization inside
        //     will be init many times too
        public virtual void Preload()
        {
        }

        //
        // Summary:
        //     Call this method to manual release (destroy) the instance.
        public void Dispose()
        {
            if (!destroying)
            {
                destroying = true;
                OnDispose();
                __instance = default(T);
            }
        }

        //
        // Summary:
        //     Override this and set all field which are reference type to null to release RAM
        //     because GC will not care for type of this Singleton.
        protected virtual void OnDispose()
        {
        }
    }
}