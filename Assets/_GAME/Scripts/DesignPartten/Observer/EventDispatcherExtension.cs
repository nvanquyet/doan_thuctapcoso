using System;
using UnityEngine;

namespace VawnWuyest
{
    internal class EventDisableListener : EventListenerBase
    {
        private void OnEnable()
        {
            if (listener != null)
            {
                listener(obj: true);
            }
        }

        private void OnDisable()
        {
            if (listener != null)
            {
                listener(obj: false);
            }
        }
    }
    internal class EventDestroyListener : EventListenerBase
{
    private void Awake()
    {
        if (listener != null)
        {
            listener(obj: true);
        }
    }

    private void OnDestroy()
    {
        if (listener != null)
        {
            listener(obj: false);
        }
    }
}
    public static class EventDispatcherExtension
    {
        //
        // Summary:
        //     Short call of "EventDispatcher.Instance.AddListener", this will auto add a component
        //     into caller gameobject
        //
        //     If "untilDisable" = true, observer will use OnEnable to register listener and
        //     OnDisable to remove listener, else use Awake and OnDestroy
        //
        // Parameters:
        //   untilDisable:
        //     If true, observer will use OnDisable to remove listener, else will use OnDestroy
        public static void AddListener<T>(this MonoBehaviour mono, Action<T> action, bool untilDisable = true) where T : IEventParam
        {
            if (untilDisable)
            {
                GetOrAddComponent<T, EventDisableListener>(mono, action);
            }
            else
            {
                GetOrAddComponent<T, EventDestroyListener>(mono, action);
            }
        }

        //
        // Summary:
        //     Short call of "EventDispatcher.Instance.AddListener", this will auto add a component
        //     into caller gameobject
        //
        //     If "untilDisable" = true, observer will use OnEnable to register listener and
        //     OnDisable to remove listener, else use Awake and OnDestroy
        //
        // Parameters:
        //   untilDisable:
        //     If true, observer will use OnDisable to remove listener, else will use OnDestroy
        public static void AddListener<T>(this MonoBehaviour mono, Action action, bool untilDisable = true) where T : IEventParam
        {
            if (untilDisable)
            {
                GetOrAddComponent<T, EventDisableListener>(mono, action);
            }
            else
            {
                GetOrAddComponent<T, EventDestroyListener>(mono, action);
            }
        }

        //
        // Summary:
        //     Short call of "EventDispatcher.Instance.Dispatch" with key is T as IEventParams
        public static void Dispatch<T>(this MonoBehaviour mono) where T : IEventParam
        {
            Service<EventDispatcher>.Instance.Dispatch<T>();
        }

        //
        // Summary:
        //     Short call of "EventDispatcher.Instance.Dispatch" with key is T as IEventParams
        public static void Dispatch<T>(this MonoBehaviour mono, T para) where T : IEventParam
        {
            Service<EventDispatcher>.Instance.Dispatch(para);
        }

        private static void GetOrAddComponent<T, TS>(MonoBehaviour mono, Action<T> action) where T : IEventParam where TS : EventListenerBase
        {
            GetOrAddComponent<TS>(mono).SetListener(action);
        }

        private static void GetOrAddComponent<T, TS>(MonoBehaviour mono, Action action) where T : IEventParam where TS : EventListenerBase
        {
            GetOrAddComponent<TS>(mono).SetListener<T>(action);
        }

        private static T GetOrAddComponent<T>(MonoBehaviour mono) where T : EventListenerBase
        {
            T val = mono.GetComponent<T>();
            if ((UnityEngine.Object)null == (UnityEngine.Object)val)
            {
                val = mono.gameObject.AddComponent<T>();
            }

            return val;
        }
    }

    public class EventListenerBase : MonoBehaviour
    {
        protected Action<bool> listener;

        public void SetListener<T>(Action<T> Action) where T : IEventParam
        {
            Service<EventDispatcher>.Instance.AddListener(Action);
            listener = (Action<bool>)Delegate.Combine(listener, (Action<bool>)delegate (bool active)
            {
                if (active)
                {
                    Service<EventDispatcher>.Instance.AddListener(Action);
                }
                else
                {
                    Service<EventDispatcher>.Instance.RemoveListener(Action);
                }
            });
        }

        public void SetListener<T>(Action Action) where T : IEventParam
        {
            Service<EventDispatcher>.Instance.AddListener<T>(Action);
            listener = (Action<bool>)Delegate.Combine(listener, (Action<bool>)delegate (bool active)
            {
                if (active)
                {
                    Service<EventDispatcher>.Instance.AddListener<T>(Action);
                }
                else
                {
                    Service<EventDispatcher>.Instance.RemoveListener<T>(Action);
                }
            });
        }
    }
}