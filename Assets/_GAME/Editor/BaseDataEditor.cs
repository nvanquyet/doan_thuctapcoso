using UnityEditor;
using UnityEngine;
namespace VawnWuyest.Editor
{
    [CustomEditor(typeof(ScriptableObject), true)]
    public class BaseDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            //serializedObject.Update();
            base.OnInspectorGUI();
            if(target is Data.IBaseDataAction) {
                var baseData = target as Data.IBaseDataAction;
                if (GUILayout.Button("Validate Key")) baseData.OnValidateKey();
                if (GUILayout.Button("Validate Value")) baseData.OnValidateValue();
                if (GUILayout.Button("Validate Data")) baseData.OnValidateData();
            }
        }
    }
}