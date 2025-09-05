using UnityEngine;

[CreateAssetMenu(fileName = nameof(StructureCellsData), menuName = "Resources/" + nameof(CellData) + "/" + nameof(StructureCellsData))]
public class StructureCellsData : CellData
{
    public EDirection frontOfSpawningCell = EDirection.Down;
    public Surface.Scale length = Surface.Scale.S1;
    public Surface.Scale width = Surface.Scale.S1;
}