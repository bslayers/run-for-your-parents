using UnityEditor;
using UnityEngine;

public abstract class Structure : MonoBehaviour
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
    //TODO
    /// <summary>
    /// 
    /// </summary>
    /// <param name="of"></param>
    /// <param name="with"></param>
    public void UpdateMaterial(GameObject[] of, Material with)
    {
        if (with == null) { return; }

        foreach (GameObject mesh in of)
        {
            if (mesh == null) { continue; }
            mesh.GetComponent<MeshRenderer>().material = with;
        }
    }


    public abstract void UpdateAllMaterial();


    #endregion


    #region Events


    #endregion

    #region Editor
#if UNITY_EDITOR
    protected void OnValidate()
    {
        EditorApplication.delayCall += () =>
        {
            UpdateAllMaterial();
        };
    }


#endif
    #endregion

}