using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityDisplayer : DisplayerSO
{
    #region Variables

    private ChunkManager chunkManager;

    private List<Cell> possiblePlaces;

    [SerializeField]
    private int forgivenEntitiesSpawingRadius = 3;


    [Header("Entities")]
    [SerializeField]
    private EntityInfo organismeDetector;
    [SerializeField]
    private EntityInfo chaser;


    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        chunkManager = GetComponent<ChunkManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Methods

    public void DisplayEntities(List<Cell> cells, Vector2Int playerCell)
    {
        if (chunkManager == null) { chunkManager = GetComponent<ChunkManager>(); }

        possiblePlaces = cells;
        RemoveForgivenCell(playerCell, forgivenEntitiesSpawingRadius);

        PlaceEntity(organismeDetector);
        PlaceEntity(chaser);
    }

    private void RemoveForgivenCell(Vector2Int center, int range)
    {
        int minX = center.x - range, maxX = center.x + range;
        int minZ = center.y - range, maxZ = center.y + range;

        for (int i = 0; i < possiblePlaces.Count; ++i)
        {
            Vector2Int coord = possiblePlaces[i].Coords;

            if (coord.x < minX || coord.x > maxX) { continue; }
            if (coord.y < minZ || coord.y > maxZ) { continue; }

            possiblePlaces.RemoveAt(i);
            --i;
        }
    }

    private void PlaceEntity(EntityInfo entity)
    {
        for (int i = 0; i < entity.nbEntities; ++i)
        {

            Cell cell;
            do
            {
                if (possiblePlaces.Count == 0) { return; }
                int index = UnityEngine.Random.Range(0, possiblePlaces.Count);
                cell = possiblePlaces[index];

                if (CanPlaceEntity(cell)) { break; }
                else
                {
                    possiblePlaces.Remove(cell);
                }
            } while (true);

            PlaceEntity(cell, entity);
        }
    }

    private bool CanPlaceEntity(Cell cell)
    {
        foreach (Direction dir in Direction.directions4List)
        {
            if (cell.neighbours[dir.direction]?.info.decor == DecorType.StreetLamp) { return false; }
        }

        return true;
    }

    private void PlaceEntity(Cell spawn, EntityInfo entity)
    {
        GameObject prefab = Instantiate(entity.entity, spawn.transform);
        chunkManager.PlaceObjectInChunk(prefab);
        RemoveForgivenCell(spawn.Coords, entity.noOtherEntitiesInRadius);
    }


    #endregion

    #region Coroutine


    #endregion

    #region Events


    #endregion

    #region Editor


    #endregion

}

[Serializable]
public class EntityInfo
{
    [RequiredReference]
    public GameObject entity;
    public int noOtherEntitiesInRadius = 5;
    public int nbEntities = 1;
}