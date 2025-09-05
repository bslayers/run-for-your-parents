#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AddInfoAttribute))]
public class AddInfoPropertyAttribute : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUI.GetPropertyHeight(property, label);

        if (property.objectReferenceValue != null)
        {
            height += EditorGUIUtility.singleLineHeight * 2f + 2;
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var requiredAttr = (RequiredReferenceAttribute)attribute;

        Rect propertyRect = new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(property, label, true));
        EditorGUI.PropertyField(propertyRect, property, true);

        if (property.objectReferenceValue != null)
        {
            Rect helpBoxRect = new Rect(
                position.x,
                propertyRect.y + propertyRect.height + 2f,
                position.width,
                EditorGUIUtility.singleLineHeight * 2f
            );

            EditorGUI.HelpBox(helpBoxRect, requiredAttr.Message, MessageType.Info);
        }
    }
}
#endif