using UnityEngine;

public class Chunk : MonoBehaviour
{
    #region Variables

    [DisableField]
    public Vector2Int coord;
    public Transform grounds;
    public Transform structures;
    public Transform items;
    public Transform entities;


    #endregion

    #region Accessors


    #endregion


    #region Built-in


    #endregion

    #region Methods

    public void ResetChunk(NoGenerationMask mask)
    {
        DestroyAllObject(grounds);
        DestroyAllObject(items);
        if ((mask & NoGenerationMask.Structure) == 0) { DestroyAllObject(structures); }
        if ((mask & NoGenerationMask.Entity) == 0) { DestroyAllObject(entities); }
    }

    private void DestroyAllObject(Transform obj)
    {
        for (int i = obj.childCount - 1; i >= 0; --i)
        {
            DestroyImmediate(obj.GetChild(i).gameObject);
        }
    }

    public void MoveStructures(Transform to)
    {
        for (int i = structures.childCount - 1; i >= 0; --i)
        {
            structures.GetChild(i).SetParent(to);
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