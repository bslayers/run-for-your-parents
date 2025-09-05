using UnityEngine;

public class StructureManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Surface.Scale length = Surface.Scale.S1;
    [SerializeField]
    private Surface.Scale width = Surface.Scale.S1;


#if UNITY_EDITOR
    [Header("For Gizmos")]
    public bool drawGizmos = true;
    [Tooltip("to help seeing the scope where the structure will spawn")]
    public Color delimitationColor = Color.green;

#endif

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


    #endregion


    #region Events


    #endregion

    #region Editor
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!drawGizmos) { return; }



        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = delimitationColor;

        Vector3 pos = new Vector3(transform.position.x - Surface.GetRealSize(length) / 2f, transform.position.y, transform.position.z - Surface.GetRealSize(width) / 2f);

        UtilGizmo.DrawRect(pos, new Vector2(Surface.GetRealSize(length), Surface.GetRealSize(width)), UtilGizmo.DrawingAxis.XZ);

        Gizmos.matrix = oldMatrix;
    }

#endif
    #endregion

}