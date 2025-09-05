using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(MapGenerator))]
public class ProceduralGenerationManager : MonoBehaviour
{
    #region Variables

    private enum GenerationType { Manual, Procedural };


    [SerializeField]
    [Tooltip("the size of the map")]
    private Vector2Int sizeOfMap;

    [SerializeField]
    [Tooltip("the type of generation\n  " + nameof(GenerationType.Manual) + ": use a json file for generate the map\n  " + nameof(GenerationType.Procedural) + ": proceduraly generate the map")]
    private GenerationType generation;

    [Header("Generators", order = -1)]

    [SerializeField]
    [Tooltip("the reference of the MapGenerator")]
    private MapGenerator mapGenerator;

    [Header("Manual generation specifications")]

    [SerializeField]
    private LevelImageMap map;

    [SerializeField]
    private GenerationInfo generationInfo = new();

    private CellInfo[,] manualMap;

    public bool alreadyGenerated = false;


    #endregion

    #region Accessors


    #endregion

    #region Structs


    #endregion

    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlaceRoad();

        ChunkManager chunkManager = GetComponent<ChunkManager>();

        if (generation == GenerationType.Manual)
        {
            StartStructureGeneration();
            StartGenerators();
            chunkManager.RecoverAndPlaceObject(mapGenerator.Structures);
        }
        chunkManager.StartChunkManager();

        Events.Instance.GenerationFinished.Invoke();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Methods

    [ContextMenu("Generate Map")]
    private int PlaceRoad()
    {
        if (alreadyGenerated) { return 1; }
#if UNITY_EDITOR
        mapGenerator.SaveStartPosition();
#endif
        switch (generation)
        {
            case GenerationType.Manual:
                map.GetMap(out manualMap);
                mapGenerator.Generate(manualMap, generationInfo, sizeOfMap);
                break;
            case GenerationType.Procedural:
                mapGenerator.Generate(sizeOfMap + new Vector2Int(0, 20), sizeOfMap);
                break;
        }
        alreadyGenerated = true;
#if UNITY_EDITOR
        mapGenerator.ResetStartPosition();
#endif

        return 0;
    }

    [ContextMenu("Reset map")]
    private void ResetMap()
    {
        mapGenerator.ClearAll();

        alreadyGenerated = false;
        GetComponent<ChunkManager>().ResetWorldChunks();

#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(this, "Reset via ContextMenu");
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    [ContextMenu("Reload map")]
    private void ReloadMap()
    {
        ResetMap();
        PlaceRoad();
    }

    private void StartStructureGeneration()
    {
        var structures = GameObject.FindGameObjectsWithTag("Structure");

        foreach (GameObject structure in structures)
        {
            IGenerator[] generators = structure.transform.GetComponentsInChildren<IGenerator>();
            foreach (IGenerator generator in generators) { generator.Generate(); }
        }

    }

    private void StartGenerators()
    {
        foreach (GameObject obj in generationInfo.listOfGenerators)
        {
            if (obj == null) { continue; }
            IGenerator[] generators = obj.transform.GetComponentsInChildren<IGenerator>();
            foreach (IGenerator generator in generators) { generator.Generate(); }
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

[Flags]
public enum NoGenerationMask
{
    Nothing = 0,
    Any = ~0,
    Structure = 1,
    Entity = 1 << 1,
}

[Serializable]
public class CellInfo
{
    public CellTypeMask mask;
    public DecorType decor;
}

[Serializable]
public class LevelImageMap
{

    [SerializeField, RequiredReference]
    private Texture2D masksImage;
    [SerializeField, RequiredReference]
    private Texture2D decorImage;

    [SerializeField]
    private List<CellTypeMask> masksImageCells;
    [SerializeField]
    private List<DecorType> decorsImageCells;

    [SerializeField]
    private CellInfo defaultCell;

    public void GetMap(out CellInfo[,] grid)
    {
        if (decorImage.height != masksImage.height) { throw new NotEqualException(nameof(decorImage), nameof(masksImage), decorImage.height, masksImage.height); }
        if (decorImage.width != masksImage.width) { throw new NotEqualException(nameof(decorImage), nameof(masksImage), decorImage.width, masksImage.width); }

        int height = masksImage.height;
        int width = masksImage.width;


        int lastLine = height - 1;
        grid = new CellInfo[width, lastLine];

        Dictionary<Color, CellTypeMask> maskDict = new();
        Dictionary<Color, DecorType> decorDict = new();


        for (int x = 0; x < width; ++x)
        {
            Color maskPixel = masksImage.GetPixel(x, lastLine);
            Color decorPixel = decorImage.GetPixel(x, lastLine);
            if (maskPixel.a != 0) { maskDict[maskPixel] = masksImageCells[x]; }
            if (decorPixel.a != 0) { decorDict[decorPixel] = decorsImageCells[x]; }

        }

        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < lastLine; ++y)
            {
                Color maskPixel = masksImage.GetPixel(x, y);
                Color decorPixel = decorImage.GetPixel(x, y);

                grid[x, y] = new() { mask = maskDict.GetValueOrDefault(maskPixel, defaultCell.mask), decor = decorDict.GetValueOrDefault(decorPixel, defaultCell.decor) };
            }
        }
    }
}

[Serializable]
public class GenerationInfo
{
    public NoGenerationMask noGenerationMask;
    [HideInInspector]
    public List<GameObject> listOfGenerators = new();
}