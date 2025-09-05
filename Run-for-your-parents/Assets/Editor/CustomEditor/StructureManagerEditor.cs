#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StructureManager), true)]
public class StructureManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;

        var structureManager = target as StructureManager;

        bool inPrefab = structureManager.gameObject.scene.name == structureManager.name;

        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;

            if (prop.name == "m_Script")
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(prop, true);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.HelpBox("All GameObject of the prefab need to be in the rect (Length*Width)", MessageType.Info);

                continue;
            }

            if (prop.name == "drawGizmos" && !inPrefab) { prop.boolValue = false; continue; }
            if (prop.name == "delimitationColor" && !inPrefab) { continue; }

            EditorGUILayout.PropertyField(prop, true);

        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif