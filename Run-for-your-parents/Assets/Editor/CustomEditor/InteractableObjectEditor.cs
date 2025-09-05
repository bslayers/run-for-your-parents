#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(InteractableObjectSO), true)]
public class InteractableObjectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        InteractableObjectSO interactable = (InteractableObjectSO)target;
        bool isOnlyEvent = target is EventOnlyInteractable;

        bool canChangeOrientation = false;
        Surface.Orientation orientation = Surface.Orientation.Horizontal;
        if (target is CabinetInteractable cabinetInteractable)
        {
            canChangeOrientation = cabinetInteractable.CanChangeOrientation;
            orientation = cabinetInteractable.Orientation;
        }

        serializedObject.Update();
        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;

        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;

            if (prop.name == "m_Script")
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(prop, true);
                EditorGUI.EndDisabledGroup();
                continue;
            }

            if (prop.name == nameof(interactable.promptMessage))
            {
                if (target is InteractableObjectWithTwoStatesSO) { continue; }
                EditorGUILayout.PropertyField(prop, true);
                CheckIsOnlyEvent(interactable, isOnlyEvent);
                continue;
            }

            if (isOnlyEvent) { continue; }


            if (prop.name == "horizontalPosition")
            {
                if (!canChangeOrientation && orientation == Surface.Orientation.Vertical) { continue; }
            }

            if (prop.name == "verticalPosition")
            {
                if (!canChangeOrientation && orientation == Surface.Orientation.Horizontal) { continue; }
            }

            EditorGUILayout.PropertyField(prop, true);

            if (prop.name == "data")
            {
                if (prop.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("Data isn't assigned", MessageType.Warning);
                }
            }

            if (prop.name == "animator")
            {
                if (prop.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("Animator isn't assigned", MessageType.Warning);
                }
            }

            //for LinkedInteractableObject
            if (prop.name == "interactableManager")
            {
                if (prop.objectReferenceValue == null)
                {
                    EditorGUILayout.HelpBox("InteractableObject isn't assigned", MessageType.Warning);
                }
            }

        }

        serializedObject.ApplyModifiedProperties();
    }

    private void CheckIsOnlyEvent(InteractableObjectSO interactable, bool isOnlyEvent)
    {
        if (!isOnlyEvent) { return; }
        EditorGUILayout.HelpBox("EventOnlyInteractable can ONLY use UnityEvents.", MessageType.Info);
        interactable.useEvents = true;
    }

}
#endif