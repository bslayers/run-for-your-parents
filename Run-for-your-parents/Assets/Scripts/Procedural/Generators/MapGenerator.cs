using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CellDisplayer))]
[RequireComponent(typeof(StructureDeterminer))]
public partial class MapGenerator : MonoBehaviour, IGenerator
{
    #region Variables

    [Header("For Generation")]

    [SerializeField, RequiredReference]
    [Tooltip("Prefab of the cell using for handle the generation")]
    private GameObject CellPrefab;

    [SerializeField, RequiredReference]
    private GameObject goalGameObject;

    private ChunkManager chunkManager;


    [SerializeField]
    [Tooltip("List of choosable WalkerConstraint for the starting point")]
    private List<WalkerConstraint> possibleWalkerConstraintForStartingCell = new();

    [SerializeField]
    private MapGenerationSpecification mapSpecification;

    [SerializeField, RequiredReference]
    private GameObject player;

    [SerializeField]
    [Tooltip("Where every ground object will be store")]
    [RequiredReference]
    private Transform grounds;
    [SerializeField]
    [RequiredReference]
    private Transform structures;

    private Vector2Int size;
    [SerializeField]
    private Vector3 startPosition = new();
    private Vector3 defaultstartPosition;

    private List<Cell> undeterminedCells = new();
    private List<Cell> roadCells = new();
    private Dictionary<Vector2Int, Cell> activeCells = new();

    private List<Walker> walkers = new();
    private List<Walker> walkersToDestroy = new();
    private List<Walker> walkersToAdd = new();
    private int nbIteration;

    private Vector2Int startCoords = new();
    private Delimitation2DInfo delimitation = new();
    private float scale = Surface.SizeForS1;
    private Dictionary<Surface.Scale, int> structureSizeWeightMap = new();
    private int structureSizeTotalWeight;
    private Dictionary<MapAttribute.RoadType, int> roadTypeWeightMap = new();
    private int roadTypeTotalWeight;
    private Dictionary<MapAttribute.IntersectionType, int> roadIntersectionWeightMap = new();

    [SerializeField, HideInInspector]
    private GenerationInfo generationInfo;


    #endregion

    #region Accessors

    public Transform Grounds { get => grounds; }
    public Transform Structures { get => structures; }

    #endregion

    #region Structs

    private struct IntersectionArgs
    {
        public bool intersectionAtLeft;
        public bool intersectionAtStraight;
        public bool intersectionAtRight;
    }

    #endregion

    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Generate(Vector2Int size, Vector2Int delimitationSize)
    {
        this.size = size;
        CalculateStartingCell();
        CalculateDelimitation(delimitationSize);


        InitGrid();
        InitSpecifications();
        Generate();
    }

    public void Generate()
    {
        walkers.Clear();

        GenerateRoad();

        StartSubGeneration();

    }

    public void Generate(CellInfo[,] grid, GenerationInfo info, Vector2Int delimitationSize)
    {
        generationInfo = info;

        size.x = grid.GetLength(1); //width
        size.y = grid.GetLength(0); // height

        CalculateStartingCell();
        CalculateDelimitation(delimitationSize);

        InitGrid();

        for (int x = 0; x < size.y; ++x)
        {
            for (int z = 0; z < size.x; ++z)
            {
                Cell cell = activeCells[new() { x = x, y = z }];
                CellInfo cellInfo = grid[x, z];
                cell.info = cellInfo;

                if (cellInfo.mask != CellTypeMask.Void && (cellInfo.mask & CellTypeMask.Structure) == 0) { undeterminedCells.Remove(cell); }
                if ((cellInfo.mask & CellTypeMask.Road) != 0 && delimitation.CompareToDelimitation(cell.Coords) == -1) { roadCells.Add(cell); }
            }
        }

        StartSubGeneration();
    }

    private void StartSubGeneration()
    {
        PlaceGoal();

        GetComponent<StructureDeterminer>().StartDetermining(undeterminedCells, size, generationInfo);

        GetComponent<CellDisplayer>().DisplayMap(activeCells.Values.ToList(), delimitation, generationInfo);

        if ((generationInfo.noGenerationMask & NoGenerationMask.Entity) == 0) { GetComponent<EntityDisplayer>().DisplayEntities(roadCells, startCoords); }

    }


    #endregion

    #region Methods

    private void InitGrid()
    {
        chunkManager = GetComponent<ChunkManager>();

        ClearAll();
        grounds.transform.position = startPosition;
        //CalculateStartingCell();
        InitAllCell();
    }

    private void InitSpecifications()
    {
        mapSpecification.GetStructureSizeWeightMap(ref structureSizeWeightMap);
        structureSizeTotalWeight = structureSizeWeightMap.Values.Sum();

        mapSpecification.GetRoadTypeWeightMap(ref roadTypeWeightMap);
        roadTypeTotalWeight = roadTypeWeightMap.Values.Sum();

        mapSpecification.GetIntersectionWeightMap(ref roadIntersectionWeightMap);
    }

    public void ClearAll()
    {
        undeterminedCells.Clear();
        activeCells.Clear();
        roadCells.Clear();
        Transform groundTransform = grounds.transform;
        for (int i = groundTransform.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(groundTransform.GetChild(i).gameObject);
        }

        GetComponent<ChunkManager>().ResetChunks(generationInfo.noGenerationMask);
    }

    public void SaveStartPosition()
    {
        defaultstartPosition = startPosition;
    }
    public void ResetStartPosition()
    {
        startPosition = defaultstartPosition;
    }

    private void CalculateStartingCell()
    {
        startCoords = new Vector2Int(size.y / 2, 0);

        startPosition = startPosition - new Vector3(startCoords.x * scale, startPosition.y, startCoords.y * scale);
    }

    private void CalculateDelimitation(Vector2Int delimitationSize)
    {
        int halfX = delimitationSize.x / 2;
        delimitation.width = new(startCoords.x - halfX, startCoords.x + halfX - 1);
        delimitation.height = new(startCoords.y, delimitationSize.y - 1);
    }

    private void InitAllCell()
    {
        for (int x = 0; x < size.y; ++x)
        {
            for (int z = 0; z < size.x; ++z)
            {
                Vector3 pos = new Vector3(x * scale, 0, z * scale);

                GameObject cellObject = Instantiate(CellPrefab, grounds.transform);
                cellObject.SetActive(false);
                cellObject.transform.localPosition = pos;
                chunkManager.PlaceObjectInChunk(cellObject);

                Cell cell = cellObject.GetComponent<Cell>();
                cell.Coords = new Vector2Int(x, z);
                undeterminedCells.Add(cell);
                activeCells.Add(cell.Coords, cell);
            }
        }

        foreach (Cell cell in activeCells.Values)
        {
            FindNeighbours(cell);
        }
    }

    private void FindNeighbours(Cell cell)
    {
        foreach (Direction dir in Direction.directions8List)
        {
            cell.neighbours[dir.direction] = GetCell(cell.Coords + dir.vector);
        }
    }

    private Cell GetCell(Vector2Int coords)
    {
        Cell cell = null;
        activeCells.TryGetValue(coords, out cell);

        return cell;
    }




    private void GenerateRoad()
    {
        WalkerConstraint constraint = possibleWalkerConstraintForStartingCell[Random.Range(0, possibleWalkerConstraintForStartingCell.Count)];
        CreateWalker(startCoords, constraint);
        //CreateIntersection(startCoords, constraint, new() { intersectionAtStraight = true });
        //UpdateWalkersList();
        nbIteration = 0;

        while (!IsGenerationFinished())
        {
            nbIteration++;
            if (nbIteration > 40) { Debug.Log("Stop by maxIteration reached"); return; }
            IterateWalkers();
        }
    }

    private void CreateWalker(Cell onCell, WalkerConstraint withConstraint)
    {
        if (onCell == null) { return; }
        walkers.Add(new Walker(this, withConstraint, onCell));
    }

    private void CreateWalker(Vector2Int atCoords, WalkerConstraint withConstraint)
    {
        Cell cell = activeCells.GetValueOrDefault(atCoords, null);
        CreateWalker(cell, withConstraint);
    }

    private bool IsGenerationFinished()
    {
        return walkers.Count == 0;
    }

    private void IterateWalkers()
    {
        Debug.Log($"nb walkers stayed: {walkers.Count}");

        Walker walker = walkers[0];

        while (!walker.NextStep()) { }


        //UpdateWalkersList();
    }

    private void UpdateWalkersList()
    {
        foreach (Walker walker in walkersToDestroy)
        {
            walkers.Remove(walker);
        }

        foreach (Walker walker in walkersToAdd)
        {
            walkers.Add(walker);
        }

        walkersToDestroy.Clear();
        walkersToAdd.Clear();
    }

    public void OnWalkerStoppedWalking(Walker walker, StoppingCause cause)
    {
        if (!IsValidWalker(walker)) { return; }

        if (cause == StoppingCause.EncounterRoad || cause == StoppingCause.OutOfMap) { DestroyWalker(walker); }

        WalkerConstraint constraint = walker.walkerConstraint;
        Vector2Int coords = walker.currentCell.Coords;

        switch (cause)
        {
            case StoppingCause.IntersectionAtLeft:
                CreateIntersection(coords, ref constraint, MapAttribute.IntersectionType.LeftIntersection);
                break;
            case StoppingCause.IntersectionAtRight:
                CreateIntersection(coords, ref constraint, MapAttribute.IntersectionType.RightIntersection);
                break;
            default:
                CreateIntersection(coords, constraint);
                DestroyWalker(walker);
                break;
        }

    }

    private bool IsValidWalker(Walker walker)
    {
        return walker != null & walkers.Contains(walker);
    }

    private void DestroyWalker(Walker walker)
    {
        walkers.Remove(walker);
    }

    private void CreateIntersection(Vector2Int coordsOfIntersection, WalkerConstraint constraintOfPreviousWalker)
    {
        IntersectionArgs args = new();
        for (; !(args.intersectionAtLeft && args.intersectionAtStraight && args.intersectionAtRight);)
        {
            args.intersectionAtLeft = Random.Range(0, MapGenerationSpecification.MAX_WEIGHT) < mapSpecification.leftIntersectionWeight;
            args.intersectionAtRight = Random.Range(0, MapGenerationSpecification.MAX_WEIGHT) < mapSpecification.rightIntersectionWeight;
            args.intersectionAtStraight = Random.Range(0, MapGenerationSpecification.MAX_WEIGHT) < mapSpecification.straightIntersectionWeight;
        }

        CreateIntersection(coordsOfIntersection, constraintOfPreviousWalker, args);
    }

    private void CreateIntersection(Vector2Int coordsOfIntersection, WalkerConstraint constraintOfPreviousWalker, IntersectionArgs intersectionArgs)
    {
        WalkerConstraint leftConstraint = new();
        WalkerConstraint straightConstraint = new();
        WalkerConstraint rightConstraint = new();

        if (intersectionArgs.intersectionAtLeft)
        {
            leftConstraint.directionToMove = Direction.directionsMap[constraintOfPreviousWalker.directionToMove.RotateDirection90(true)];
            DetermineConstraint(ref leftConstraint, constraintOfPreviousWalker, MapAttribute.IntersectionType.LeftIntersection);

            UpdateMaxSteps(ref rightConstraint, constraintOfPreviousWalker.createWalkwayAtEndLeft, CanCreateWalkway());

            CreateWalker(constraintOfPreviousWalker.roadPartAtLeft ? coordsOfIntersection + Direction.LEFT.vector : coordsOfIntersection, rightConstraint);
        }

        if (intersectionArgs.intersectionAtRight)
        {
            rightConstraint.directionToMove = Direction.directionsMap[constraintOfPreviousWalker.directionToMove.RotateDirection90(false)];
            DetermineConstraint(ref rightConstraint, constraintOfPreviousWalker, MapAttribute.IntersectionType.RightIntersection);

            UpdateMaxSteps(ref rightConstraint, CanCreateWalkway(), constraintOfPreviousWalker.createWalkwayAtEndRight);

            CreateWalker(constraintOfPreviousWalker.roadPartAtRight ? coordsOfIntersection + Direction.RIGHT.vector : coordsOfIntersection, rightConstraint);
        }

        if (intersectionArgs.intersectionAtStraight)
        {
            straightConstraint.directionToMove = constraintOfPreviousWalker.directionToMove;

            DetermineConstraint(ref straightConstraint, constraintOfPreviousWalker, MapAttribute.IntersectionType.StraightIntersection);

            bool walkwayAtRightBeginning = intersectionArgs.intersectionAtRight ? rightConstraint.createWalkwayAtLeft : false;
            bool walkwayAtLeftBeginning = intersectionArgs.intersectionAtLeft ? leftConstraint.createWalkwayAtRight : false;

            UpdateMaxSteps(ref straightConstraint, walkwayAtLeftBeginning, walkwayAtRightBeginning);

            CreateWalker(coordsOfIntersection, straightConstraint);
        }
    }

    private void CreateIntersection(Vector2Int coordsOfIntersection, ref WalkerConstraint constraintOfPreviousWalker, MapAttribute.IntersectionType directionOfIntersection)
    {
        WalkerConstraint newConstraint = new();
        newConstraint.roadType = constraintOfPreviousWalker.roadType;

        if (directionOfIntersection == MapAttribute.IntersectionType.LeftIntersection)
        {
            constraintOfPreviousWalker.maxStepsAtRight = constraintOfPreviousWalker.maxStepsAtLeft;
            DetermineConstraint(ref newConstraint, constraintOfPreviousWalker, MapAttribute.IntersectionType.LeftIntersection);

            UpdateMaxSteps(ref newConstraint, newConstraint.createWalkwayAtLeft, newConstraint.createWalkwayAtLeft);

            CreateWalker(constraintOfPreviousWalker.roadPartAtLeft ? coordsOfIntersection + Direction.LEFT.vector : coordsOfIntersection, newConstraint);
        }
        else if (directionOfIntersection == MapAttribute.IntersectionType.RightIntersection)
        {
            constraintOfPreviousWalker.maxStepsAtLeft = constraintOfPreviousWalker.maxStepsAtRight;
            DetermineConstraint(ref newConstraint, constraintOfPreviousWalker, MapAttribute.IntersectionType.RightIntersection);

            UpdateMaxSteps(ref newConstraint, newConstraint.createWalkwayAtRight, newConstraint.createWalkwayAtRight);

            CreateWalker(constraintOfPreviousWalker.roadPartAtRight ? coordsOfIntersection + Direction.RIGHT.vector : coordsOfIntersection, newConstraint);
        }
    }

    private void DetermineConstraint(ref WalkerConstraint newConstraint, WalkerConstraint constraintOfPreviousWalker, MapAttribute.IntersectionType intersectionType)
    {
        DetermineNbStepsByStructure(ref newConstraint);

        DetermineWalkway(ref newConstraint);

        DetermineRoad(ref newConstraint, constraintOfPreviousWalker, intersectionType);

        if (newConstraint.createWalkwayAtEndLeft) { ++newConstraint.maxStepsAtLeft; }
        if (newConstraint.createWalkwayAtEndRight) { ++newConstraint.maxStepsAtRight; }
    }

    private void DetermineNbStepsByStructure(ref WalkerConstraint newConstraint)
    {
        newConstraint.maxStepsAtRight = GenerateWalkerSteps();
        newConstraint.maxStepsAtLeft = GenerateWalkerSteps();
    }

    private void DetermineWalkway(ref WalkerConstraint newConstraint)
    {
        newConstraint.createWalkwayAtLeft = CanCreateWalkway();
        newConstraint.createWalkwayAtRight = CanCreateWalkway();

        newConstraint.createWalkwayAtEndLeft = CanCreateWalkway();
        newConstraint.createWalkwayAtEndRight = CanCreateWalkway();
    }

    private void DetermineRoad(ref WalkerConstraint newConstraint, WalkerConstraint constraintOfPreviousWalker, MapAttribute.IntersectionType intersectionType)
    {
        if (intersectionType == MapAttribute.IntersectionType.StraightIntersection)
        {
            newConstraint.roadType = constraintOfPreviousWalker.roadType;
            newConstraint.roadPartAtLeft = constraintOfPreviousWalker.roadPartAtLeft;
            newConstraint.roadPartAtRight = constraintOfPreviousWalker.roadPartAtRight;
            return;
        }

        newConstraint.roadType = roadTypeWeightMap.Keys.ToArray()[IGenerator.ChooseItem(roadTypeWeightMap.Values.ToArray(), roadTypeTotalWeight)];

        if (newConstraint.roadType == MapAttribute.RoadType.SingleRoad) { return; }

        if (intersectionType == MapAttribute.IntersectionType.LeftIntersection)
        {
            newConstraint.roadPartAtLeft = true;
        }
        else if (intersectionType == MapAttribute.IntersectionType.RightIntersection)
        {
            newConstraint.roadPartAtRight = true;
        }
    }

    private void UpdateMaxSteps(ref WalkerConstraint newConstraint, bool walkwayAtLeft, bool walkwayAtRight)
    {
        if (walkwayAtLeft) { ++newConstraint.maxStepsAtLeft; }
        if (walkwayAtRight) { ++newConstraint.maxStepsAtRight; }

        CheckAccurateSteps(ref newConstraint);
    }

    private void CheckAccurateSteps(ref WalkerConstraint newConstraint)
    {
        newConstraint.maxStepsAtLeft = newConstraint.maxStepsAtLeft < mapSpecification.minStepsAllowed ? mapSpecification.minStepsAllowed : newConstraint.maxStepsAtLeft;
        newConstraint.maxStepsAtRight = newConstraint.maxStepsAtRight < mapSpecification.minStepsAllowed ? mapSpecification.minStepsAllowed : newConstraint.maxStepsAtRight;

        int distanceBetweenIntersection = newConstraint.maxStepsAtLeft - newConstraint.maxStepsAtRight;

        if (distanceBetweenIntersection > 0 && distanceBetweenIntersection <= mapSpecification.mindistanceBetweenIntersection)
        {
            newConstraint.maxStepsAtRight = newConstraint.maxStepsAtLeft;
        }
        if (distanceBetweenIntersection < 0 && distanceBetweenIntersection >= -mapSpecification.mindistanceBetweenIntersection)
        {
            newConstraint.maxStepsAtLeft = newConstraint.maxStepsAtRight;
        }

    }

    private int GenerateWalkerSteps()
    {
        int nbSteps = 0;

        int nbStructure = Random.Range(mapSpecification.minNbStructure, mapSpecification.maxNbStructure);

        for (int i = 0; i < nbStructure; ++i)
        {
            Surface.Scale scale = structureSizeWeightMap.Keys.ToArray()[IGenerator.ChooseItem(structureSizeWeightMap.Values.ToArray(), structureSizeTotalWeight)];
            nbSteps += (int)Surface.scaleMap[scale];
        }

        return nbSteps;
    }

    private bool CanCreateWalkway()
    {
        return Random.Range(0, MapGenerationSpecification.MAX_WEIGHT) < mapSpecification.walkwayWeight;
    }

    public void CellTypeChanged(Cell cell)
    {
        if (cell?.info.mask == CellTypeMask.Void) { return; }
        undeterminedCells.Remove(cell);
    }

    private void PlaceGoal()
    {
        Cell cell = GetNearRoadAtCenterOfLine(size.x - 1);

        if (cell == null) { return; }

        GameObject goal = Instantiate(goalGameObject, cell.transform);

        roadCells.Remove(cell);
    }

    private Cell GetNearRoadAtCenterOfLine(int z)
    {
        int x = size.y / 2;

        for (int i = 0; i < x; ++i)
        {
            Cell cell = activeCells[new(x + i, z)];

            if ((cell.info.mask & CellTypeMask.Road) != 0) { return cell; }

            cell = activeCells[new(x - i, z)];

            if ((cell.info.mask & CellTypeMask.Road) != 0) { return cell; }
        }

        return null;
    }





    #endregion


    #region Events


    #endregion

    #region Editor

    #endregion
}

public class Delimitation2DInfo
{

    public MinMaxInt width;
    public MinMaxInt height;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="coord"></param>
    /// <returns>
    /// -1 : in delimitation;<br/>
    /// 0 : at delimitation;<br/>
    /// 1 : out of delimitation;
    /// </returns>
    public int CompareToDelimitation(Vector2Int coord)
    {
        int coordX = coord.x, coordY = coord.y;

        bool inWidth = width.In(coordX), inHeight = height.In(coordY),
        atWidth = width.AtLimit(coordX), atHeight = height.AtLimit(coordY);

        if (inWidth && inHeight) { return -1; }

        if ((atWidth && inHeight) || (atHeight && inWidth) || (atWidth && atHeight)) { return 0; }

        return 1;
    }
}