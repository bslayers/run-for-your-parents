using System.Collections.Generic;
using UnityEngine;

public class StructureDeterminer : DisplayerSO
{
    private class CellGroups
    {
        public List<StructureCellInfo> structureCellInfos;
    }

    private struct StructureCellInfo
    {
        public Cell cell;
        public bool hasOnlyRoadNeighbour;
    }

    #region Variables

    private List<CellGroups> cellGroups;

    private Dictionary<Vector2Int, List<StructureCellsData>> structureDatasDict;

    public StructureCellsData ground;

    #endregion

    #region Accessors


    #endregion


    #region Built-in


    #endregion

    #region Methods

    public void StartDetermining(List<Cell> undeterminedCells, Vector2Int sizeOfMap, GenerationInfo info)
    {
        DisplayGround(undeterminedCells);

        if ((info.noGenerationMask & NoGenerationMask.Structure) != 0) { return; }

        FillCellGroups(undeterminedCells, sizeOfMap);

        RecoverStructureDatas();

        //TODO: choose a size for each structure

        DisplayStructure();
    }

    /// <summary>
    /// Start structure determining behaviour 
    /// </summary>
    /// <param name="undeterminedCells">the list of possible cell where place structure</param>
    public void StartDetermining(List<Cell> undeterminedCells, Vector2Int sizeOfMap)
    {
        StartDetermining(undeterminedCells, sizeOfMap, new() { noGenerationMask = NoGenerationMask.Any });
    }

    private void FillCellGroups(List<Cell> cells, Vector2Int sizeOfMap)
    {
        Matrix2D<bool> visitedCells = new(sizeOfMap, false);
        cellGroups = new();

        while (cells.Count > 0)
        {
            CreateGroup(ref cells, ref visitedCells);
        }

    }

    private void CreateGroup(ref List<Cell> cells, ref Matrix2D<bool> visitedCells)
    {
        CellGroups group = new() { structureCellInfos = new() };

        FloodFill(ref cells, ref group, ref visitedCells);

        cellGroups.Add(group);
    }

    private void FloodFill(ref List<Cell> cells, ref CellGroups group, ref Matrix2D<bool> visitedCells)
    {
        Queue<Cell> queue = new();
        queue.Enqueue(cells[0]);
        visitedCells[cells[0].Coords] = true;

        while (queue.Count > 0)
        {
            Cell currentCell = queue.Dequeue();
            cells.Remove(currentCell);
            StructureCellInfo info = new() { cell = currentCell, hasOnlyRoadNeighbour = true };

            foreach (Direction direction in Direction.directions4List)
            {
                Cell neighbourCell = currentCell.neighbours[direction.direction];
                if (neighbourCell == null) { continue; }
                if (visitedCells[neighbourCell.Coords]) { continue; }

                if (neighbourCell.info.mask == CellTypeMask.Void || neighbourCell.info.mask == CellTypeMask.Structure)
                {
                    queue.Enqueue(neighbourCell);
                }
                else { info.hasOnlyRoadNeighbour = false; }

                visitedCells[neighbourCell.Coords] = true;
            }

            group.structureCellInfos.Add(info);
        }
    }

    private void RecoverStructureDatas()
    {
        List<StructureCellsData> list = GetComponent<CellDisplayer>().StructureDatas;

        InitStrucureDatasDict();

        foreach (StructureCellsData data in list)
        {
            structureDatasDict[new() { x = (int)Surface.scaleMap[data.length], y = (int)Surface.scaleMap[data.width] }].Add(data);
        }
    }

    private void InitStrucureDatasDict()
    {
        structureDatasDict = new();
        for (int x = 1; x < 5; ++x)
        {
            for (int y = 1; y < 5; ++y)
            {
                structureDatasDict[new() { x = x, y = y }] = new();
            }
        }
    }

    private void DisplayStructure()
    {
        foreach (CellGroups group in cellGroups)
        {
            foreach (StructureCellInfo structure in group.structureCellInfos)
            {
                //For testing recovering
            }
        }
    }

    private void DisplayGround(List<Cell> list)
    {
        foreach (Cell cell in list)
        {
            DisplayCell(cell, ground);
        }
    }


    #endregion

    #region Coroutine


    #endregion

    #region Events


    #endregion

    #region Editor


    #endregion

}