using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawner : MonoBehaviour, IGenerator
{
    #region Variables

    [Tooltip("List of spawnable objects with their respective weights.")]
    public SpawnerPrefabConfiguration spawnableObjects;

    [Tooltip("List of spawn points where objects can be spawned.")]
    public List<Transform> spawnPoints;

    [Tooltip("Maximum number of spawn points to use for spawning objects.")]
    public int maxSpawnPoints = 2;

    private List<Transform> availableSpawnPoints;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    public void Generate()
    {
        availableSpawnPoints = new List<Transform>(spawnPoints);
        if (spawnableObjects == null) { Debug.LogWarning($"spawner of {gameObject} has no {nameof(spawnableObjects)}"); return; }
        int totalWeight = spawnableObjects.spawnableObjects.Sum(item => item?.weight ?? 0);
        int spawnCount = 0;

        while (availableSpawnPoints.Count > 0 && spawnCount < maxSpawnPoints)
        {
            int randomNumber = Random.Range(0, totalWeight);

            SpawnableProperties selected = spawnableObjects.spawnableObjects[0];
            foreach (var item in spawnableObjects.spawnableObjects)
            {
                randomNumber -= item.weight;
                if (randomNumber < 0)
                {
                    selected = item;
                    break;
                }
            }

            int index = Random.Range(0, availableSpawnPoints.Count);
            Transform point = availableSpawnPoints[index];
            Vector3 spawnPosition = point.position + Vector3.up * selected.distanceFromPrefabBase;

            if (selected?.prefab == null) { continue; }
            GameObject spawnedObject = Instantiate(selected.prefab, point.position, point.rotation);
            spawnedObject.transform.SetParent(transform);
            availableSpawnPoints.RemoveAt(index);

            IGenerator[] generators = spawnedObject.GetComponents<IGenerator>();
            foreach (IGenerator generator in generators)
            {
                generator.Generate();
            }


            spawnCount++;
        }
    }

    #endregion

    #region Methods


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion

}
