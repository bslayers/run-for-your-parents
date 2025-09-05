#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CellConnection), true)]
public class CellConnectionPropertyDrawer : PropertyDrawer
{

    private const float Padding = 4f;
    private float labelHeight = EditorGUIUtility.singleLineHeight;
    private string[,] fieldNames = new string[3, 3] {
            { nameof(CellConnection.upLeft), nameof(CellConnection.up), nameof(CellConnection.upRight) },
            { nameof(CellConnection.left), nameof(CellConnection.center), nameof(CellConnection.right) },
            { nameof(CellConnection.downLeft), nameof(CellConnection.down), nameof(CellConnection.downRight) }
        };

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded) return labelHeight;

        return (labelHeight * 2 + Padding) * 3 + labelHeight + Padding * 2;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.isExpanded = EditorGUI.Foldout(
            new Rect(position.x, position.y, position.width, labelHeight),
            property.isExpanded,
            label,
            true
        );

        if (!property.isExpanded) return;

        EditorGUI.indentLevel++;


        Rect boxRect = new Rect(position.x, position.y + labelHeight + 2, position.width, GetPropertyHeight(property, label) - labelHeight - 2);
        GUI.Box(boxRect, GUIContent.none, EditorStyles.helpBox);

        float cellWidth = (position.width - 4 * Padding - 16) / 3;



        for (int row = 0; row < 3; ++row)
        {
            for (int col = 0; col < 3; ++col)
            {
                string fieldName = fieldNames[row, col];
                var field = property.FindPropertyRelative(fieldName);
                if (field == null) continue;

                float baseX = position.x + 12 + col * (cellWidth + Padding);
                float baseY = position.y + labelHeight + 8 + row * (labelHeight * 2 + Padding);

                Rect fullRect = new Rect(baseX, baseY, cellWidth, labelHeight);

                //Colorize field
                Color bgColor = GetColorForCellType(field.enumValueFlag);
                EditorGUI.DrawRect(fullRect, bgColor);

                //Field name
                Rect labelRect = new Rect(baseX, baseY, cellWidth, labelHeight);
                EditorGUI.LabelField(labelRect, fieldNames[row, col], UtilEditor.DarkMiniLabel);

                //Enum field
                Rect fieldRect = new Rect(baseX, baseY + labelHeight, cellWidth, labelHeight);
                EditorGUI.PropertyField(fieldRect, field, GUIContent.none);
            }
        }

        EditorGUI.indentLevel--;
    }

    private Color GetColorForCellType(int bitfield)
    {
        Color color = new Color(0, 0, 0, 0.3f);

        if ((bitfield & (int)CellTypeMask.Road) != 0) { color += new Color(1f, 0, 0, 0f); }
        if ((bitfield & (int)CellTypeMask.Structure) != 0) { color += new Color(0, 0, 1f, 0f); }
        if ((bitfield & (int)CellTypeMask.Walkway) != 0) { color += new Color(0, 1f, 0, 0f); }

        return color;
    }
}
#endif