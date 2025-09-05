using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IndexedEnumList<E, TEnum> where TEnum : Enum
{
    #region Variables

    public List<E> list = new();
    private Array keys = Enum.GetValues(typeof(TEnum));

    #endregion

    #region Accessors

    public Type GetEnumType() => typeof(TEnum);

    public E this[TEnum x]
    {
        get
        {
            return list[Array.IndexOf(Enum.GetValues(typeof(TEnum)), x)];
        }
        set
        {
            if (list[Array.IndexOf(Enum.GetValues(typeof(TEnum)), x)].Equals(value)) { return; }

            list[Array.IndexOf(Enum.GetValues(typeof(TEnum)), x)] = value;
        }
    }

    public Array Keys { get => keys; }

    #endregion
}

public class IndexedMenuTypeList<E> : IndexedEnumList<E, MenuData.MenuType> { }

[Serializable]
public class IndexedMenuTypeList : IndexedMenuTypeList<MenuSO> { }

public class IndexedMemberList<E> : IndexedEnumList<E, BodyMemberType> { }

[Serializable]
public class BodyMemberList : IndexedMemberList<BodyMember> { }

[Serializable]
public class IndexedItemTypeList : IndexedEnumList<GameObject, ItemData.ITEM_TYPE> { }

[Serializable]
public class IndexedHandleTypeList : IndexedEnumList<GameObject, HandleMaterialData.HandleType> { }

[Serializable]
public class IndexedHandManagerList : IndexedEnumList<HandManager, Hand> { }

[Serializable]
public class IndexedBoolHandList : IndexedEnumList<bool, Hand> { }

public class IndexedDirectionList<E> : IndexedEnumList<E, EDirection> { }

[Serializable]
public class CellDirectionList : IndexedDirectionList<DelimitationCellData> { }