#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public abstract class IndexedEnumListPropertyDrawer<TEnum> : PropertyDrawer where TEnum : Enum
{
    protected Type enumType = typeof(TEnum);

    protected static string listName = nameof(IndexedEnumList<int, Surface.Scale>.list);

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return UtilPropertyDrawer.CalculateListHeight(property, listName, UtilPropertyDrawer.normalDrawingInfos);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        UtilPropertyDrawer.DrawIndexedEnumList(position, property, new GUIContent(label), listName, UtilPropertyDrawer.normalDrawingInfos, Enum.GetNames(enumType));
    }
}

[CustomPropertyDrawer(typeof(IndexedMenuTypeList), true)]
public class IndexedMenuTypeListPropertyDrawer : IndexedEnumListPropertyDrawer<MenuData.MenuType> { }

[CustomPropertyDrawer(typeof(IndexedHandleTypeList), true)]
public class IndexedHandleTypeListPropertyDrawer : IndexedEnumListPropertyDrawer<HandleMaterialData.HandleType> { }

[CustomPropertyDrawer(typeof(IndexedItemTypeList), true)]
public class IndexedItemTypeListPropertyDrawer : IndexedEnumListPropertyDrawer<ItemData.ITEM_TYPE> { }

[CustomPropertyDrawer(typeof(BodyMemberList), true)]
public class ImageMemberListPropertyDrawer : IndexedEnumListPropertyDrawer<BodyMemberType> { }

[CustomPropertyDrawer(typeof(IndexedHandManagerList), true)]
public class IndexedHandListPropertyDrawer : IndexedEnumListPropertyDrawer<Hand> { }

[CustomPropertyDrawer(typeof(IndexedBoolHandList), true)]
public class IndexedBoolHandListPropertyDrawer : IndexedEnumListPropertyDrawer<Hand> { }

[CustomPropertyDrawer(typeof(CellDirectionList), true)]
public class CellDirectionListPropertyDrawer : IndexedEnumListPropertyDrawer<EDirection> { }

#endif