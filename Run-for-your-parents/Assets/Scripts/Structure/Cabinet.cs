using UnityEngine;

public class Cabinet : Structure
{
    #region Variables
    [Tooltip("Information for material customisation of the cabinet")]
    [RequiredReference, SerializeField]
    private CabinetMaterialData materialData;

    [Header("For customization")]
    [Tooltip("List of CabinetInteractables to update their handle")]
    public CabinetInteractable[] cabinetInteractables = { };
    [Tooltip("List of GameObject to update their material with body material")]
    public GameObject[] bodies = { };



    #endregion

    #region Accessors

    public CabinetMaterialData MaterialData
    {
        get => materialData;
        set
        {
            if (value == materialData) { return; }
            materialData = value;
            UpdateAllMaterial();
            
        }
    }


    #endregion


    #region Built-in


    #endregion

    #region Methods
    public override void UpdateAllMaterial()
    {
        if (materialData == null) { return; }

        UpdateHandlesOfCabinet(materialData.handleMaterialData);
        UpdateMaterial(bodies, materialData.bodyMaterial);
    }

    private void UpdateHandlesOfCabinet(HandleMaterialData data)
    {
        if (data == null) { return; }
        foreach (CabinetInteractable interactable in cabinetInteractables)
        {
            interactable.SetHandle(data);
        }
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion

}