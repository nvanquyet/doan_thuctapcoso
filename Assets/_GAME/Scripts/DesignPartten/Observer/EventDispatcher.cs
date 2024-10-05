using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootingGame
{
    public interface IEventParam
    {

    }

    internal sealed class IntEventObserver : EventObserver<int, object>
    {
    }
    
    public class EventDispatcher : Service<EventDispatcher>
    {
        private IntEventObserver _intEventObserver;

        private ParamEventObserver _paramEventObserver;

        private IntEventObserver IntEventObserver
        {
            get
            {
                if (_intEventObserver != null)
                {
                    return _intEventObserver;
                }

                _intEventObserver = new IntEventObserver();
                return _intEventObserver;
            }
        }

        private ParamEventObserver ParamEventObserver
        {
            get
            {
                if (_paramEventObserver != null)
                {
                    return _paramEventObserver;
                }

                _paramEventObserver = new ParamEventObserver();
                return _paramEventObserver;
            }
        }

        protected override void Initialize()
        {
        }

        public void SetLogEnable(bool enable)
        {
            ParamEventObserver.EnableDebugLog = enable;
        }

        //
        // Summary:
        //     Becarefull. Should use when your game has only one scene, or all your game logic
        //     place in only one scene (except logoscene).
        public void RemoveAllListener()
        {
            if (_paramEventObserver != null)
            {
                _paramEventObserver.RemoveAllListener();
            }

            GC.Collect();
        }

        protected override void OnDispose()
        {
            if (_paramEventObserver != null)
            {
                _paramEventObserver = null;
            }
        }

        //
        // Summary:
        //     IntEventObserver listener
        public void Dispatch(int key)
        {
            IntEventObserver.Dispatch(key);
        }

        //
        // Summary:
        //     IntEventObserver listener
        public void AddListener(int key, Action eventCallback)
        {
            IntEventObserver.AddListener(key, eventCallback);
        }

        //
        // Summary:
        //     IntEventObserver listener
        public void RemoveListener(int key, Action eventCallback)
        {
            IntEventObserver.RemoveListener(key, eventCallback);
        }

        //
        // Summary:
        //     Remove ALL callback listener by `key`. Be carefully, use at OnDestroy only to
        //     free RAM.
        public void RemoveListener(int key)
        {
            IntEventObserver.RemoveListener(key);
        }

        //
        // Summary:
        //     ParamEventObserver listener
        public void Dispatch<T>() where T : IEventParam
        {
            ParamEventObserver.Dispatch<T>();
        }

        //
        // Summary:
        //     ParamEventObserver listener
        public void Dispatch<T>(T eventParams) where T : IEventParam
        {
            ParamEventObserver.Dispatch(eventParams);
        }

        //
        // Summary:
        //     ParamEventObserver listener
        public void AddListener<T>(Action<T> eventCallback) where T : IEventParam
        {
            ParamEventObserver.AddListener(eventCallback);
        }

        //
        // Summary:
        //     ParamEventObserver listener
        public void AddListener<T>(Action eventCallback) where T : IEventParam
        {
            ParamEventObserver.AddListener<T>(eventCallback);
        }

        //
        // Summary:
        //     ParamEventObserver listener
        public void RemoveListener<T>(Action<T> eventCallback) where T : IEventParam
        {
            ParamEventObserver.RemoveListener(eventCallback);
        }

        //
        // Summary:
        //     ParamEventObserver listener
        public void RemoveListener<T>(Action eventCallback) where T : IEventParam
        {
            ParamEventObserver.RemoveListener<T>(eventCallback);
        }

        //
        // Summary:
        //     Remove ALL callback listener by `key`. Be carefully, use at OnDestroy only to
        //     free RAM.
        public void RemoveListener<T>() where T : IEventParam
        {
            ParamEventObserver.RemoveListener<T>();
        }
    }
    internal sealed class ParamEventObserver : EventObserver<Type, IEventParam>
    {
        public void AddListener<T>(Action<T> eventCallback) where T : IEventParam
        {
            if (eventCallback == null)
            {
                DebugLogError("Cannot AddListener for key " + typeof(T).Name + " with null eventCallback");
                return;
            }

            Action<IEventParam> action = delegate (IEventParam param)
            {
                eventCallback?.Invoke((param != null) ? ((T)param) : default(T));
            };
            AddByHashCode(typeof(T), eventCallback.GetHashCode(), action);
        }

        public void RemoveListener<T>(Action<T> eventCallback) where T : IEventParam
        {
            if (eventCallback == null)
            {
                DebugLogError("Cannot RemoveListener for key " + typeof(T).Name + " with null eventCallback");
            }
            else
            {
                RemoveByHashCode(typeof(T), eventCallback.GetHashCode());
            }
        }

        public void AddListener<T>(Action eventCallback) where T : IEventParam
        {
            if (eventCallback == null)
            {
                DebugLogError("Cannot AddListener for key " + typeof(T).Name + " with null eventCallback");
                return;
            }

            Action<IEventParam> action = delegate
            {
                eventCallback?.Invoke();
            };
            AddByHashCode(typeof(T), eventCallback.GetHashCode(), action);
        }

        public void RemoveListener<T>(Action eventCallback) where T : IEventParam
        {
            if (eventCallback == null)
            {
                DebugLogError("Cannot RemoveListener for key " + typeof(T).Name + " with null eventCallback");
            }
            else
            {
                RemoveByHashCode(typeof(T), eventCallback.GetHashCode());
            }
        }

        public void RemoveListener<T>() where T : IEventParam
        {
            RemoveListener(typeof(T));
        }

        public void Dispatch<T>() where T : IEventParam
        {
            Dispatch(typeof(T), null);
        }

        public void Dispatch<T>(T param) where T : IEventParam
        {
            Dispatch(typeof(T), param);
        }
    }


    internal class EventObserver<K, V>
    {
        private readonly Dictionary<K, Dictionary<int, Action<V>>> observerDictionary = new Dictionary<K, Dictionary<int, Action<V>>>();

        public bool EnableDebugLog { get; set; }

        protected void DebugLog(string msg)
        {
            if (EnableDebugLog)
            {
                Debug.Log("[EventDispatcher] " + msg);
            }
        }

        protected void DebugLogError(string msg)
        {
            Debug.LogError("[EventDispatcher] " + msg);
        }

        public void AddListener(K key, Action<V> eventCallback)
        {
            if (eventCallback == null)
            {
                DebugLogError("Cannot AddListener for key " + typeof(K).Name + " with null eventCallback");
            }
            else
            {
                AddByHashCode(key, eventCallback.GetHashCode(), eventCallback);
            }
        }

        public void AddListener(K key, Action eventCallback)
        {
            if (eventCallback == null)
            {
                DebugLogError("Cannot AddListener for key " + typeof(K).Name + " with null eventCallback");
                return;
            }

            int hashCode = eventCallback.GetHashCode();
            Action<V> action = delegate
            {
                eventCallback();
            };
            AddByHashCode(key, hashCode, action);
        }

        public void RemoveListener(K key, Action<V> eventCallback)
        {
            if (eventCallback == null)
            {
                DebugLogError("Cannot RemoveListener for key " + typeof(K).Name + " with null eventCallback");
            }
            else
            {
                RemoveByHashCode(key, eventCallback.GetHashCode());
            }
        }

        public void RemoveListener(K key, Action eventCallback)
        {
            if (eventCallback == null)
            {
                DebugLogError("Cannot RemoveListener for key " + typeof(K).Name + " with null eventCallback");
            }
            else
            {
                RemoveByHashCode(key, eventCallback.GetHashCode());
            }
        }

        //
        // Summary:
        //     Remove ALL callback listener by `key`. Be carefully, use at OnDestroy only to
        //     free RAM.
        public void RemoveListener(K key)
        {
            RemoveByKey(key);
        }

        //
        // Summary:
        //     Remove all listener. Be carefully, use at OnDestroy or OnApplicationQuiting only
        //     to free RAM.
        public void RemoveAllListener()
        {
            observerDictionary.Clear();
        }

        protected void AddByHashCode(K key, int hashCode, Action<V> action)
        {
            if (EnableDebugLog)
            {
                DebugLog($"Register Key: {key} - {hashCode}");
            }

            if (!observerDictionary.TryGetValue(key, out var value))
            {
                value = new Dictionary<int, Action<V>>();
                observerDictionary[key] = value;
            }

            value[hashCode] = action;
        }

        protected void RemoveByHashCode(K key, int hashCode)
        {
            if (observerDictionary.TryGetValue(key, out var value))
            {
                value.Remove(hashCode);
                if (EnableDebugLog)
                {
                    DebugLog($"UnRegister Key: {key} - {hashCode}");
                }
            }
        }

        private void RemoveByKey(K key)
        {
            if (observerDictionary.Remove(key) && EnableDebugLog)
            {
                DebugLog($"UnRegister All of Key: {key}");
            }
        }

        public void Dispatch(K key, V obj = default(V))
        {
            if (key == null)
            {
                DebugLogError("Cannot dispatch with null key of type=" + typeof(K).Name);
            }
            else
            {
                if (!observerDictionary.ContainsKey(key) || !observerDictionary.TryGetValue(key, out var value) || value == null || value.Values.Count == 0)
                {
                    return;
                }

                if (EnableDebugLog)
                {
                    DebugLog($"Dispatch total_events={value.Count}, key={key})");
                }

                List<KeyValuePair<int, Action<V>>> list = new List<KeyValuePair<int, Action<V>>>(value.Count);
                Dictionary<int, Action<V>>.Enumerator enumerator = value.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Value != null)
                    {
                        list.Add(new KeyValuePair<int, Action<V>>(enumerator.Current.Key, enumerator.Current.Value));
                    }
                }

                foreach (KeyValuePair<int, Action<V>> item in list)
                {
                    if (EnableDebugLog)
                    {
                        DebugLog($"Invoke callback: key={key}, action={item.Key})");
                    }

                    item.Value(obj);
                }
            }
        }
    }
}