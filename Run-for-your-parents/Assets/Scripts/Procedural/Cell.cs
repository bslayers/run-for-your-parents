using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    #region Variables

    public CellInfo info;

    public CellData cellData;


    private Vector2Int coords = new(-1, -1);
    public Dictionary<EDirection, Cell> neighbours = new Dictionary<EDirection, Cell>()
    {
        [EDirection.Up] = null,
        [EDirection.UpRight] = null,
        [EDirection.Right] = null,
        [EDirection.DownRight] = null,
        [EDirection.Down] = null,
        [EDirection.DownLeft] = null,
        [EDirection.Left] = null,
        [EDirection.UpLeft] = null,
    };


    #endregion

    #region Accessors

    public Vector2Int Coords
    {
        get => coords;
        set
        {
            if (value == coords) { return; }
            coords = value;
            name = $"Cell{coords}";
        }
    }

    public bool IsDetermined
    {
        get => info.mask != CellTypeMask.Void;
    }

    #endregion

    #region Built-in


    #endregion

    #region Methods


    #endregion

}