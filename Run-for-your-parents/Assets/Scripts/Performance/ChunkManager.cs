using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ChunkManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Transform player;

    [SerializeField]
    [RequiredReference]
    private Transform chunksParent;

    [SerializeField]
    [RequiredReference]
    private GameObject chunkPrefab;


    [SerializeField]
    private int chunkSize;

    [SerializeField]
    private int viewDistanceInChunks = 2;
    private Vector2Int lastChunkCoord;

    private Dictionary<Vector2Int, Chunk> worldChunks = new Dictionary<Vector2Int, Chunk>();

    [Header("NavMesh")]

    [SerializeField]
    private List<NavMeshSurfaceInfo> surfaces;

    private Vector3 boundSizeForBaking = Vector3.one;

    private List<Transform> surveyedEntities = new();


    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        worldChunks.Clear();
        for (int i = chunksParent.childCount - 1; i >= 0; --i)
        {
            Transform child = chunksParent.GetChild(i);
            Chunk chunk = child.GetComponent<Chunk>();

            if (chunk == null) { continue; }
            worldChunks.Add(chunk.coord, chunk);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Events.Instance.GenerationFinished.AddListener(OnEventsGenerationFinished);

    }

    void LateUpdate()
    {
        Vector2Int currentCoord = ConvertPositionToCoord(player.position);

        if (currentCoord != lastChunkCoord)
        {
            StartCoroutine(UpdateChunks(currentCoord));
        }
    }

    #endregion

    #region Methods

    [ContextMenu("Destroy chunks")]
    public void ResetWorldChunks()
    {
        for (int i = chunksParent.childCount - 1; i >= 0; --i) { DestroyImmediate(chunksParent.GetChild(i).gameObject); }
        worldChunks.Clear();
    }

    [ContextMenu("Put out structures")]
    private void PutOutStructure()
    {
        Transform structures = GetComponent<MapGenerator>().Structures;
        for (int i = chunksParent.childCount - 1; i >= 0; --i)
        {
            chunksParent.GetChild(i).GetComponent<Chunk>()?.MoveStructures(structures);
        }
    }

    public int StartChunkManager()
    {
        worldChunks.Clear();

        for (int i = chunksParent.childCount - 1; i >= 0; --i)
        {
            Transform child = chunksParent.GetChild(i);
            Chunk chunk = child.GetComponent<Chunk>();

            if (chunk == null) { continue; }
            worldChunks.Add(chunk.coord, chunk);
        }

        Game.Instance.North = GameObject.FindGameObjectWithTag("NorthGoal").transform;

        NavMesh.RemoveAllNavMeshData();
        foreach (var surface in surfaces)
        {
            surface.currentNavMeshData = new NavMeshData();
            surface.currentNavMeshDataInstance = NavMesh.AddNavMeshData(surface.currentNavMeshData);
            surface.navMeshSurface.navMeshData = surface.currentNavMeshData;
            surface.navMeshSurface.BuildNavMesh();
        }

        StartCoroutine(DelayStartChunks());

        return 0;
    }

    private Vector2Int ConvertPositionToCoord(Vector3 position)
    {
        return new Vector2Int(
            Mathf.FloorToInt(position.x / chunkSize),
            Mathf.FloorToInt(position.z / chunkSize)
        );
    }

    private Vector3 ConvertCoordToPosition(Vector2Int coord)
    {
        return new Vector3(coord.x * chunkSize, 0, coord.y * chunkSize);
    }

    private Chunk CreateChunk(Vector2Int coord)
    {
        GameObject chunkObject = Instantiate(chunkPrefab, chunksParent);
        chunkObject.transform.position = new Vector3(coord.x * chunkSize, 0, coord.y * chunkSize);
        chunkObject.name = $"Chunk_{coord.x}_{coord.y}";
        Chunk chunk = chunkObject.GetComponent<Chunk>();
        chunk.coord = coord;

        return chunk;
    }

    private void UpdateVisibleChunks(Vector2Int playerChunk)
    {
        HashSet<Vector2Int> neededChunks = new HashSet<Vector2Int>();

        for (int dx = -viewDistanceInChunks; dx <= viewDistanceInChunks; ++dx)
        {
            for (int dz = -viewDistanceInChunks; dz <= viewDistanceInChunks; ++dz)
            {
                Vector2Int coord = new Vector2Int(playerChunk.x + dx, playerChunk.y + dz);
                if (!worldChunks.ContainsKey(coord)) { continue; }

                neededChunks.Add(coord);

                worldChunks[coord].gameObject.SetActive(true);
            }
        }

        // Deactivate chunks not in view
        foreach (var kvp in worldChunks)
        {
            if (!neededChunks.Contains(kvp.Key))
            {
                kvp.Value.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateVisibleChunks(Vector3 playerPos)
    {
        Vector2Int playerChunk = ConvertPositionToCoord(playerPos);

        UpdateVisibleChunks(playerChunk);
    }

    public int RecoverAndPlaceObject(Transform from)
    {
        for (int i = from.childCount - 1; i >= 0; --i)
        {
            Transform obj = from.GetChild(i);
            PlaceObjectInChunk(obj);
        }
        return 0;
    }

    public void PlaceObjectInChunk(GameObject obj)
    {
        PlaceObjectInChunk(obj.transform);
    }

    public void PlaceObjectInChunk(Transform obj)
    {
        Vector2Int coord = ConvertPositionToCoord(obj.position);
        Chunk chunk = worldChunks.GetValueOrDefault(coord, null);
        if (chunk == null)
        {
            chunk = CreateChunk(coord);
            worldChunks[coord] = chunk;
        }

        if (obj.CompareTag("Item")) { obj.SetParent(chunk.items); }
        else if (obj.CompareTag("Entity")) { obj.SetParent(chunk.entities); }
        else if (obj.CompareTag("Structure")) { obj.SetParent(chunk.structures); }
        else if (obj.CompareTag("Ground")) { obj.SetParent(chunk.grounds); }
        else
        {
            List<Transform> list = new();
            for (int i = 0; i < obj.childCount; ++i) { list.Add(obj.GetChild(i)); }
            foreach (Transform child in list)
            {
                PlaceObjectInChunk(child);
            }
        }

    }

    public void ResetChunks(NoGenerationMask mask)
    {
        for (int i = chunksParent.childCount - 1; i >= 0; --i)
        {
            chunksParent.GetChild(i).GetComponent<Chunk>()?.ResetChunk(mask);
        }

    }

    public void UpdateStoppingEntityInChunk(Transform thing, ref Vector2Int currentCoord)
    {
        Vector2Int coord = ConvertPositionToCoord(thing.position);

        if (coord == currentCoord) { return; }

        if (!worldChunks.ContainsKey(coord))
        {
            worldChunks[coord] = CreateChunk(coord);
        }

        if (thing.CompareTag("Item")) { thing.SetParent(worldChunks[coord].items); }
        else if (thing.CompareTag("Entity")) { thing.SetParent(worldChunks[coord].entities); }

        currentCoord = coord;
        return;
    }

    private void UpdateMovingEntitiesInChunk()
    {
        foreach (Transform entity in surveyedEntities)
        {
            if (entity == null) { continue; }
            UpdateThingChunk(entity);
        }
    }

    private void UpdateThingChunk(Transform thing)
    {
        Vector2Int coord = ConvertPositionToCoord(thing.position);
        if (!worldChunks.ContainsKey(coord)) { Debug.Log($"{coord} doesn't exit"); }
        if (thing.CompareTag("Item")) { thing.SetParent(worldChunks[coord].items); }
        else if (thing.CompareTag("Entity")) { thing.SetParent(worldChunks[coord].entities); }
    }

    public void RemoveEntityFromSurveyList(Transform thing)
    {
        surveyedEntities.Remove(thing);
    }







    #endregion

    #region Coroutine

    IEnumerator<AsyncOperation> BuildAsync(NavMeshSurfaceInfo surface, Vector3 currentChunk)
    {
        NavMeshSurface navMeshSurface = surface.navMeshSurface;
        var settings = NavMesh.GetSettingsByID(navMeshSurface.agentTypeID);

        List<NavMeshBuildSource> sources = new();
        NavMeshBuilder.CollectSources(
            null, navMeshSurface.layerMask, navMeshSurface.useGeometry,
            navMeshSurface.defaultArea, new List<NavMeshBuildMarkup>(), sources
        );

        Bounds bounds = new(currentChunk, boundSizeForBaking);

        var newData = new NavMeshData();
        navMeshSurface.navMeshData = newData;

        var bakeOp = NavMeshBuilder.UpdateNavMeshDataAsync(newData, settings, sources, bounds);
        yield return bakeOp;

        var newInstance = NavMesh.AddNavMeshData(newData, navMeshSurface.transform.position, navMeshSurface.transform.rotation);


        surface.currentNavMeshDataInstance.Remove();


        surface.currentNavMeshDataInstance = newInstance;
        surface.currentNavMeshData = newData;
    }


    IEnumerator UpdateChunks(Vector2Int currentCoord)
    {
        UpdateVisibleChunks(currentCoord);
        UpdateMovingEntitiesInChunk();
        lastChunkCoord = currentCoord;
        
        yield break;
    }

    IEnumerator DelayStartChunks()
    {
        yield return new WaitForSeconds(.5f);

        UpdateVisibleChunks(player.transform.position);
    }




    #endregion

    #region Events

    private void OnEventsGenerationFinished()
    {
        //Debug.Log("Generation finish");
    }

    public void SurveyEntity(Transform entity, bool survey, ref Vector2Int currentCoord)
    {
        if (!survey) { UpdateStoppingEntityInChunk(entity, ref currentCoord); RemoveEntityFromSurveyList(entity); return; }

        surveyedEntities.Add(entity);
    }

    void OnDestroy()
    {
        NavMesh.RemoveAllNavMeshData();
    }



    #endregion

    #region Editor


    #endregion

}

[Serializable]
public class NavMeshSurfaceInfo
{
    public NavMeshSurface navMeshSurface;
    [DisableField]
    public NavMeshData currentNavMeshData;
    [DisableField]
    public NavMeshDataInstance currentNavMeshDataInstance;
}