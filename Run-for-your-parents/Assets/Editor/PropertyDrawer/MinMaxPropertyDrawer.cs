#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public abstract class MinMaxPropertyDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw the prefix label and get remaining rect
        position = EditorGUI.PrefixLabel(position, label);

        // Save indent and set to 0 to avoid extra spacing
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Get sub-properties
        SerializedProperty minProp = property.FindPropertyRelative("min");
        SerializedProperty maxProp = property.FindPropertyRelative("max");

        // Calculate field widths and spacing
        float labelWidth = 28f; // Width for "Min" / "Max" labels
        float fieldSpacing = 5f;

        float totalFieldWidth = (position.width - fieldSpacing) / 2;
        float fieldWidth = totalFieldWidth - labelWidth;

        // Define rects
        Rect minLabelRect = new Rect(position.x, position.y, labelWidth, position.height);
        Rect minFieldRect = new Rect(minLabelRect.xMax, position.y, fieldWidth, position.height);

        Rect maxLabelRect = new Rect(minFieldRect.xMax + fieldSpacing, position.y, labelWidth, position.height);
        Rect maxFieldRect = new Rect(maxLabelRect.xMax, position.y, fieldWidth, position.height);

        // Draw fields
        EditorGUI.LabelField(minLabelRect, "Min");
        EditorGUI.PropertyField(minFieldRect, minProp, GUIContent.none);

        EditorGUI.LabelField(maxLabelRect, "Max");
        EditorGUI.PropertyField(maxFieldRect, maxProp, GUIContent.none);

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

}

[CustomPropertyDrawer(typeof(MinMaxFloat), true)]
public class MinMaxFloatPropertyDrawer : MinMaxPropertyDrawer { }

[CustomPropertyDrawer(typeof(MinMaxInt), true)]
public class MinMaxIntPropertyDrawer : MinMaxPropertyDrawer { }

#endif