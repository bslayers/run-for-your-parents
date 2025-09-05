using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SpawnerPrefabConfiguration), menuName = "Resources/" + nameof(SpawnerPrefabConfiguration))]
public class SpawnerPrefabConfiguration : ScriptableObject
{
    [Tooltip("List of spawnable objects with their respective weights.")]
    public List<SpawnableProperties> spawnableObjects;

    [ContextMenu("Calcul all distance")]
    /// <summary>
    /// Calculates the distance from the base of the prefab to its pivot point for all spawnable objects.
    /// </summary>
    public void CalculAll()
    {
        if (spawnableObjects == null) { return; }
        foreach (var spawnableProperties in spawnableObjects)
        {
            spawnableProperties.Calcul();
        }
    }
}