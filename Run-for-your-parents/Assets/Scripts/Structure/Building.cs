using UnityEngine;


[DisallowMultipleComponent]
public class Building : Structure
{
    #region Variables

    [RequiredReference]
    public BuildingMaterialData materialData;

    [Header("For customization")]
    public GameObject[] externalWalls = { };
    public GameObject[] externalFloors = { };
    public GameObject[] externalCeillings = { };
    public GameObject[] externalStairs = { };
    public GameObject[] roofs = { };
    public GameObject[] columns = { };


    [Header("Rooms in building")]

    [SerializeField]
    private Room[] rooms = { };

    #endregion

    #region Accessors

    #endregion


    #region Built-in


    #endregion

    #region Methods


    public override void UpdateAllMaterial()
    {
        if (materialData == null) { return; }

        UpdateMaterial(externalWalls, materialData.wallMaterial);
        UpdateMaterial(externalCeillings, materialData.ceillingMaterial);
        UpdateMaterial(externalFloors, materialData.floorMaterial);
        UpdateMaterial(roofs, materialData.roofMaterial);
        UpdateMaterial(columns, materialData.columnMaterial);
        UpdateMaterial(externalStairs, materialData.stairMaterial);

    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion

}