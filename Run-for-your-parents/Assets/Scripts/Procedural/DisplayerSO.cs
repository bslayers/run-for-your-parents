using System.Collections.Generic;
using UnityEngine;

public abstract class DisplayerSO : MonoBehaviour
{
    #region Variables


    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {

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

    protected static void DisplayCell(Cell cell, CellData cellData)
    {
        DisplayCell(cell, cellData, Vector3.zero);
    }

    protected static void DisplayCell(Cell cell, CellData cellData, Vector3 offset)
    {
        DisplayCell(cell, cellData, offset, true);
    }

    protected static void DisplayCell(Cell cell, CellData cellData, Vector3 offset, bool searchGenerator)
    {
        if (cellData == null || cellData.prefab == null) { return; }
        GameObject prefab;
        DisplayCell(cell, cellData, offset, out prefab);

        if (!searchGenerator) { return; }

        IGenerator[] generators = prefab.transform.GetComponentsInChildren<IGenerator>();
        foreach (IGenerator generator in generators) { generator.Generate(); }
    }

    protected static void DisplayCell(Cell cell, CellData cellData, Vector3 offset, bool searchGenerator, ref List<GameObject> listWhereAdd)
    {
        if (cellData == null || cellData.prefab == null) { return; }
        GameObject prefab;
        DisplayCell(cell, cellData, offset, out prefab);

        if (!searchGenerator) { return; }

        listWhereAdd.Add(prefab);
        /*
        IGenerator[] generators = prefab.transform.GetComponentsInChildren<IGenerator>();
        foreach (IGenerator generator in generators) { generator.Generate(); }
        */
    }

    private static void DisplayCell(Cell cell, CellData cellData, Vector3 offset, out GameObject prefab)
    {
        prefab = Instantiate(cellData.prefab, cell.gameObject.transform, true);
        prefab.transform.Rotate(new Vector3(0f, cellData.rotation, 0f));
        prefab.transform.localPosition = offset;
        cell.gameObject.SetActive(true);
    }


    #endregion

    #region Coroutine


    #endregion

    #region Events


    #endregion

    #region Editor


    #endregion

}