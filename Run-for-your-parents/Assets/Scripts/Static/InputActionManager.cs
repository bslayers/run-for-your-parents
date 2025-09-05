using UnityEngine;
using UnityEngine.InputSystem;

public class InputActionManager : MonoBehaviour
{
    #region Variables
    public static InputActionManager Instance;
    public InputActionAsset inputAction;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Methods


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion

}