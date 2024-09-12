using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace VawnWuyest.Data
{

    [System.Serializable]
    public struct BaseDataStruct<K, V>
    {
        public K key;
        public V value;
    }


    public interface IBaseDataAction
    {
        void OnValidateKey();
        void OnValidateValue();
        void OnValidateData();

    }

    public abstract class BaseData<K, V> : ScriptableObject, IBaseDataAction
    {
        [SerializeField] protected BaseDataStruct<K, V>[] _data;

        protected Dictionary<K, V> _dataDict;

        public Dictionary<K, V> DataDict
        {
            get
            {
                if (_dataDict == null)
                {
                    _dataDict = new Dictionary<K, V>();
                    foreach (var item in _data)
                    {
                        _dataDict.Add(item.key, item.value);
                    }
                }
                return _dataDict;
            }
        }

        public V[] GetAllValue()
        {
            return DataDict.Values.ToArray();
        }
        public abstract void OnValidateKey();
        public abstract void OnValidateValue();
        public abstract void OnValidateData();
    }


    public abstract class BaseIntKeyData<V> : BaseData<int, V>
    {
        protected abstract string Path { get; }
        public override void OnValidateData()
        {
#if UNITY_EDITOR
            string[] prefabGuids = UnityEditor.AssetDatabase.FindAssets("t:Prefab", new[] { Path });
            for (int i = 0; i < _data.Length; i++)
            {
                string prefabPath = UnityEditor.AssetDatabase.GUIDToAssetPath(prefabGuids[Mathf.Clamp(i, 0, prefabGuids.Length - 1)]);

                GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if(prefab != null && prefab.TryGetComponent<V>(out var data))
                {
                    _data[i].key = i;
                    _data[i].value = data;
                }
            }
            Debug.Log("Validate Data Success");
            //Set Dirty 
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public override void OnValidateKey()
        {
            if (_data == null || _data.Length == 0) return;
            for (int i = 0; i < _data.Length; i++) _data[i].key = i;
            UnityEditor.EditorUtility.SetDirty(this);
            Debug.Log("Validate Key Success");
        }

        public override void OnValidateValue()
        {
#if UNITY_EDITOR
            string[] prefabGuids = UnityEditor.AssetDatabase.FindAssets("t:Prefab", new[] { Path });
            for (int i = 0; i < _data.Length; i++)
            {
                string prefabPath = UnityEditor.AssetDatabase.GUIDToAssetPath(prefabGuids[Mathf.Clamp(i, 0, prefabGuids.Length - 1)]);

                GameObject prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                if(prefab != null && prefab.TryGetComponent<V>(out var data))
                {
                    _data[i].key = i;
                    _data[i].value = data;
                }
            }
            Debug.Log("Validate Data Success");
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
}