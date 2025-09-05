using UnityEngine;

[CreateAssetMenu(fileName = nameof(StructureCellsData), menuName = "Resources/" + nameof(CellData) + "/" + nameof(DecorCellData))]
public class DecorCellData : CellData
{
    public DecorType decor;
}

public enum DecorType
{
    Void,
    StreetLamp,
    RandomDecor,
    NoDelimitation,
}