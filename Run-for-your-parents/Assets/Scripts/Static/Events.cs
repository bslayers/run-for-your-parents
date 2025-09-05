using UnityEngine;
using UnityEngine.Events;

public class Events : MonoBehaviour
{

    #region Variables
    [Tooltip("Singleton instance of the Events class")]
    public static Events Instance;

    #endregion

    #region Built-In

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

    #region Events
    [HideInInspector]
    public UnityEvent<string> SceneChanged;

    [HideInInspector]
    public UnityEvent GenerationFinished;


    #endregion

}