using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CellDisplayer : DisplayerSO
{
    #region Variables

    [SerializeField]
    private float yOffsetWhenWalkwayForDecor = 0.1f;
    private Vector3 offset;

    [SerializeField]
    [Tooltip("The list of CellData for generation")]
    private List<CellData> cellDatas = new();

    [SerializeField]
    [Tooltip("The list of DecorCellData for generation")]
    private List<CellData> decorCellDatas = new();

    [SerializeField]
    private List<StructureCellsData> structuresData = new();

    [SerializeField]
    private CellDirectionList delimitationCells = new();

    [SerializeField]
    [RequiredReference]
    private SpawnerData randomDecors;

    private Delimitation2DInfo delimitation;

    [SerializeField, HideInInspector]
    private GenerationInfo generationInfo;


    private const double PERFECT_MATCHING_SCORE = 3 * 9;


    #endregion

    #region Accessors

    public List<StructureCellsData> StructureDatas { get => structuresData; }


    #endregion


    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        //RecoverAllCellData();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    #endregion

    #region Methods

#if UNITY_EDITOR
    [ContextMenu("Recover all CellData")]
    private void RecoverAllCellData()
    {
        cellDatas.Clear();
        decorCellDatas.Clear();
        string[] guids = AssetDatabase.FindAssets($"t:{nameof(CellData)}");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            CellData cell = AssetDatabase.LoadAssetAtPath<CellData>(path);

            if (cell is DecorCellData decorCell) { decorCellDatas.Add(decorCell); }
            else if (cell is StructureCellsData structureCell) { structuresData.Add(structureCell); }
            else if (cell is DelimitationCellData) { continue; }
            else { cellDatas.Add(cell); }


            if (cell.prefab == null) { Debug.LogWarning($"{cell.name} has no assigned prefab"); }
        }
    }
#endif

    /// <summary>
    /// Display all Cell of the <paramref name="cellList"/>
    /// </summary>
    /// <param name="cellList"></param>
    public void DisplayMap(List<Cell> cellList, Delimitation2DInfo delimitation, GenerationInfo info)
    {
        generationInfo = info;

        randomDecors.decorCells.CalculateWeights();
        this.delimitation = delimitation;
        offset = new(0, yOffsetWhenWalkwayForDecor, 0);

        foreach (Cell cell in cellList)
        {
            CellConnection connection;
            RecoverCellNeighbours(cell, out connection);

            cell.cellData = GetBestCellData(cell, connection);
            DisplayCell(cell, cell.cellData);
            DisplayDecor(cell, connection);

            if (delimitation.CompareToDelimitation(cell.Coords) != 0) { continue; }
            DisplayDelimitation(cell);
        }
    }

    private CellData GetBestCellData(Cell cell, CellConnection connection)
    {
        if (cell.info.mask == CellTypeMask.Void || cell.info.mask == CellTypeMask.Structure) { return null; }

        double bestScore = -1;
        CellData bestData = null;

        foreach (CellData data in cellDatas)
        {
            double score = data.connection.MatchScore(connection);
            if (score > bestScore)
            {
                bestScore = score;
                bestData = data;

                if (bestScore >= PERFECT_MATCHING_SCORE) { break; }
            }
        }

        if (bestScore <= -1)
        {
            Debug.LogWarning($"{cell.name}: No matching cell for connection : {connection}");
        }

        return bestData;
    }

    private void RecoverCellNeighbours(Cell cell, out CellConnection connection)
    {
        connection = new() { center = cell.info.mask };

        connection.upLeft = GetCellNeighbourMask(cell, EDirection.UpLeft);
        connection.up = GetCellNeighbourMask(cell, EDirection.Up);
        connection.upRight = GetCellNeighbourMask(cell, EDirection.UpRight);

        connection.left = GetCellNeighbourMask(cell, EDirection.Left);
        connection.right = GetCellNeighbourMask(cell, EDirection.Right);

        connection.downLeft = GetCellNeighbourMask(cell, EDirection.DownLeft);
        connection.down = GetCellNeighbourMask(cell, EDirection.Down);
        connection.downRight = GetCellNeighbourMask(cell, EDirection.DownRight);

        if (connection.up == 0 && connection.down != 0) { connection.up = connection.down; }
        if (connection.down == 0 && connection.up != 0) { connection.down = connection.up; }
        if (connection.left == 0 && connection.right != 0) { connection.left = connection.right; }
        if (connection.right == 0 && connection.left != 0) { connection.right = connection.left; }
    }

    private CellTypeMask GetCellNeighbourMask(Cell cell, EDirection direction)
    {
        return cell.neighbours[direction]?.info.mask ?? CellTypeMask.Void;
    }



    private void DisplayDecor(Cell cell, CellConnection connection)
    {
        switch (cell.info.decor)
        {
            case DecorType.StreetLamp:
                DisplayStreetLamp(cell, connection);
                return;
            case DecorType.RandomDecor:
                DisplayRandomDecor(cell, connection);
                return;
            default:
                return;
        }
    }

    private void DisplayStreetLamp(Cell cell, CellConnection connection)
    {
        EDirection direction = HasRoadFrom(connection);
        if (direction == EDirection.Center) { return; }

        DecorCellData decor = GetBestDecorCellData(cell, connection);
        DisplayCell(cell, decor, Vector3.zero, delimitation.CompareToDelimitation(cell.Coords) != -1, ref generationInfo.listOfGenerators);
    }

    private void DisplayRandomDecor(Cell cell, CellConnection connection)
    {
        int index = IGenerator.ChooseItem(randomDecors.decorCells.Weights, randomDecors.decorCells.TotalWeight);
        if (index == -1) { return; }
        DecorCellData decor = randomDecors.decorCells.list[index].asset;
        DisplayCell(cell, decor, Vector3.zero, delimitation.CompareToDelimitation(cell.Coords) != -1, ref generationInfo.listOfGenerators);
    }

    private EDirection HasRoadFrom(CellConnection connection)
    {
        var patternMap = connection.ConnectionsMap;
        foreach (Direction dir in Direction.directions4List)
        {
            if (patternMap[dir.direction] == CellTypeMask.Road) { return dir.direction; }
        }

        return EDirection.Center;
    }

    private DecorCellData GetBestDecorCellData(Cell cell, CellConnection connection)
    {
        double bestScore = -1;
        DecorCellData bestData = null;

        foreach (DecorCellData data in decorCellDatas)
        {
            if (data.decor != cell.info.decor) { return null; }

            double score = data.connection.MatchScore(connection);
            if (score > bestScore)
            {
                bestScore = score;
                bestData = data;

                if (bestScore >= PERFECT_MATCHING_SCORE) { break; }
            }
        }

        if (bestScore <= -1)
        {
            Debug.LogWarning($"{cell.name}: No matching cell for connection : {connection}");
        }

        return bestData;
    }

    private void DisplayDelimitation(Cell cell)
    {
        CellTypeMask mask = cell.info.mask;
        //if ((mask & CellTypeMask.Structure) != 0) { return; }
        if (cell.info.decor == DecorType.NoDelimitation) { return; }

        DelimitationCellData cellData = delimitationCells[GetDirectionFromAtLimitDelimitation(cell.Coords)];
        if (cellData == null) { return; }

        DisplayCell(cell, cellData, (mask & CellTypeMask.Road) != 0 ? Vector3.zero : offset);
    }

    private EDirection GetDirectionFromAtLimitDelimitation(Vector2Int coord)
    {
        bool atRight = delimitation.width.max == coord.x;
        bool atLeft = delimitation.width.min == coord.x;
        bool atUp = delimitation.height.max == coord.y;
        bool atDown = delimitation.height.min == coord.y;

        if (atUp && atRight) { return EDirection.UpRight; }
        if (atUp && atLeft) { return EDirection.UpLeft; }
        if (atDown && atRight) { return EDirection.DownRight; }
        if (atDown && atLeft) { return EDirection.DownLeft; }
        if (atDown) { return EDirection.Down; }
        if (atLeft) { return EDirection.Left; }
        if (atUp) { return EDirection.Up; }
        if (atRight) { return EDirection.Right; }

        return EDirection.Center;
    }


    #endregion

    #region Coroutine


    #endregion

    #region Events


    #endregion

    #region Editor


    #endregion

}