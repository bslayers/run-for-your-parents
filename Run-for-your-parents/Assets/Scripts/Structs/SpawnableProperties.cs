using UnityEngine;

[System.Serializable]
public class SpawnableProperties
{
    [Tooltip("Prefab of the object to spawn")]
    public GameObject prefab;

    [Header("Spawn Parameters")]
    [Tooltip("Weight of the object to spawn. Lesser weight means more likely to be spawned")]
    [Range(1, 99)]
    public int weight;

    public float distanceFromPrefabBase;

    /// <summary>
    /// Calculates the distance from the base of the prefab to its pivot point.
    /// TODO : Maybe it should be better to have a button that calculate for having better performance in game
    /// </summary>
    public void Calcul()
    {
        if (prefab == null) return;

        // It's necessary to be able to collect information about the prefab 
        GameObject tempObject = Object.Instantiate(prefab);

        MeshCollider meshCollider = tempObject.GetComponentInChildren<MeshCollider>();

        if (meshCollider != null)
        {
            Bounds bounds = meshCollider.bounds;
            float objectCenter = bounds.center.y;
            float objectPivot = meshCollider.transform.position.y;

            distanceFromPrefabBase = objectCenter - objectPivot;
        }
        else
        {
            distanceFromPrefabBase = 0f;
        }
        // Because we have instantiate it we need to destroy it now
        Object.DestroyImmediate(tempObject);
    }
    
}
