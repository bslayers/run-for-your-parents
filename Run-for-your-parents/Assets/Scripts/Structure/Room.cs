using UnityEngine;

public class Room : Structure
{
    #region Variables
    [RequiredReference]
    public RoomMaterialData materialData;

    [Header("For customization")]
    public GameObject[] internalwalls = { };
    public GameObject[] internalfloors = { };
    public GameObject[] internalceillings = { };
    public GameObject[] internalStairs = { };

    #endregion

    #region Accessors


    #endregion


    #region Built-in


    #endregion

    #region Methods

    public override void UpdateAllMaterial()
    {
        if (materialData == null) { return; }

        UpdateMaterial(internalwalls, materialData.wallMaterial);
        UpdateMaterial(internalceillings, materialData.ceillingMaterial);
        UpdateMaterial(internalfloors, materialData.floorMaterial);
        UpdateMaterial(internalStairs, materialData.stairMaterial);
    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion

}