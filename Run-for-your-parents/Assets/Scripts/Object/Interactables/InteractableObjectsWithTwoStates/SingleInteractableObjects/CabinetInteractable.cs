using System;
using UnityEditor;
using UnityEngine;

public class CabinetInteractable : SingleInteractableObject
{
    #region Variables

    [SerializeField]
    private HandleMaterialData data;

    [Header("Choosing handle")]
    private GameObject currentHandle;

    [SerializeField]
    protected IndexedHandleTypeList handles = new();

    [Header("Choosing Handle Orientation")]
    [SerializeField]
    private bool canChangeOrientation = true;

    [SerializeField]
    private Vector3 horizontalPosition;
    [SerializeField]
    private Vector3 verticalPosition;

    #endregion

    #region Accessors

    public bool CanChangeOrientation { get => canChangeOrientation; }
    public Surface.Orientation Orientation { get => data ? data.orientation : Surface.Orientation.Horizontal; }

    public GameObject CurrentHandle { get => currentHandle; }
    public HandleMaterialData.HandleType HandleType { get => data ? data.type : HandleMaterialData.HandleType.Old; }

    #endregion


    #region Built-in

    protected override void StartObject()
    {
        if (data != null) { UpdateHandle(); }
        base.StartObject();
    }

    #endregion

    #region Methods

    protected virtual GameObject ShowHandle(GameObject handle)
    {
        if (handle == null) { return currentHandle; }

        if (handle != currentHandle)
        {
            HideHandles();
            handle.SetActive(true);
            currentHandle = handle;
        }
        return handle;
    }

    private void UpdateHandleOrientation()
    {
        if (currentHandle == null) { return; }
        switch (data.orientation)
        {
            case Surface.Orientation.Horizontal:
                currentHandle.transform.localPosition = horizontalPosition;
                currentHandle.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case Surface.Orientation.Vertical:
                currentHandle.transform.localPosition = verticalPosition;
                currentHandle.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                break;
        }
    }

    private void HideHandles()
    {
        foreach (GameObject handle in handles.list)
        {
            if (handle == null) { continue; }
            handle.SetActive(false);
        }
    }

    private void UpdateHandle()
    {
        currentHandle = ShowHandle(handles.list[Array.IndexOf(Enum.GetValues(typeof(HandleMaterialData.HandleType)), data.type)]);
        if (currentHandle == null) { return; }

        if (canChangeOrientation)
        {
            UpdateHandleOrientation();
        }

        currentHandle.GetComponent<MeshRenderer>().material = data.material;
    }

    public void SetHandle(HandleMaterialData newData)
    {
        if (newData == null) { return; }
        data = newData;
        UpdateHandle();
    }

    #endregion

    #region Events


    #endregion

    #region Editor
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (data == null) { return; }

        EditorApplication.delayCall += () =>
        {
            ShowHandle(handles.list[Array.IndexOf(Enum.GetValues(typeof(HandleMaterialData.HandleType)), data.type)]);
            UpdateHandleOrientation();
        };
    }
#endif
    #endregion

}