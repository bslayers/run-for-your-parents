#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public static class UtilEditor
{
    public static GUIStyle BoldFoldoutStyle = new GUIStyle(EditorStyles.foldout)
    {
        fontStyle = FontStyle.Bold
    };

    private static GUIStyle _darkMiniLabel;
    public static GUIStyle DarkMiniLabel
    {
        get
        {
            if (_darkMiniLabel == null)
            {
                _darkMiniLabel = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
                _darkMiniLabel.normal.textColor = new Color(0f, 0f, 0f);
                _darkMiniLabel.fontStyle = FontStyle.Normal;
            }
            return _darkMiniLabel;
        }
    }

    public static void DrawListWithEnumIndex(SerializedProperty prop, string headerName, ref bool showBox)
    {
        showBox = EditorGUILayout.Foldout(showBox, headerName, true, BoldFoldoutStyle);

        if (showBox)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(prop, true);
            EditorGUILayout.EndVertical();
        }
    }
}

public static class UtilPropertyDrawer
{
    public static DrawingInfos normalDrawingInfos = new()
    {
        spacing = EditorGUIUtility.standardVerticalSpacing,
        lineHeight = EditorGUIUtility.singleLineHeight,
        padding = 4f
    };


    public static float CalculateListHeight(SerializedProperty property, string listName, DrawingInfos infos)
    {
        SerializedProperty list = property.FindPropertyRelative(listName);
        return CalculateListHeight(property, list, infos);
    }

    public static float CalculateListHeight(SerializedProperty property, SerializedProperty list, DrawingInfos infos)
    {
        float height = EditorGUIUtility.singleLineHeight + infos.padding * 2;

        if (!property.isExpanded) { return height; }

        for (int i = 0; i < list.arraySize; i++)
        {
            var element = list.GetArrayElementAtIndex(i);
            height += EditorGUI.GetPropertyHeight(element, true) + infos.spacing;
        }

        return height;
    }

    public static void DrawIndexedEnumList(Rect position, SerializedProperty property, GUIContent label, string fieldName, DrawingInfos infos, string[] enumNames)
    {
        SerializedProperty listProp = property.FindPropertyRelative(fieldName);

        float totalHeight = CalculateListHeight(property, listProp, infos);

        DrawBox(position, totalHeight);

        DrawFoldout(position, property, label, infos);

        if (!property.isExpanded) { return; }

        DrawEnumList(position, listProp, enumNames, infos);
    }

    public static void DrawHeader(GUIContent label, Rect position, float lineHeight)
    {
        Rect labelRect = new(position.x, position.y, position.width, lineHeight);
        EditorGUI.PrefixLabel(labelRect, label);
    }

    public static void DrawBox(Rect position, float height)
    {
        DrawBox(position, height, EditorStyles.helpBox);
    }

    public static void DrawBox(Rect position, float height, GUIStyle style)
    {
        Rect boxRect = new(position.x, position.y, position.width, height);
        GUI.Box(boxRect, GUIContent.none, style);
    }

    public static void DrawFoldout(Rect position, SerializedProperty property, GUIContent label, DrawingInfos infos)
    {
        label.text = " " + label.text;
        Rect foldoutRect = new(position.x + infos.spacing, position.y + infos.padding, position.width - infos.padding * 2, infos.lineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true, UtilEditor.BoldFoldoutStyle);
    }

    public static void DrawEnumList(Rect position, SerializedProperty listProperty, string[] enumNames, DrawingInfos infos)
    {
        float y = position.y + infos.lineHeight + infos.spacing;
        int size = enumNames.Length;

        ++EditorGUI.indentLevel;

        for (int i = 0; i < size; ++i)
        {
            if (i >= listProperty.arraySize)
                listProperty.InsertArrayElementAtIndex(i);

            Rect lineRect = new(position.x + infos.padding, y, position.width, infos.lineHeight);
            SerializedProperty element = listProperty.GetArrayElementAtIndex(i);

            EditorGUI.PropertyField(lineRect, element, new GUIContent(enumNames[i]), true);
            y += EditorGUI.GetPropertyHeight(element, true) + infos.spacing;
        }

        --EditorGUI.indentLevel;
    }
}

public struct DrawingInfos
{
    public float spacing;
    public float lineHeight;
    public float padding;
}

#endif
