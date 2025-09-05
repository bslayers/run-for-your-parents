using UnityEngine;

[RequireComponent(typeof(Player))]
public abstract class PlayerComponentSO : MonoBehaviour
{
    #region Variables

    protected Player player;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        InitPlayer();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void InitPlayer()
    {
        player = GetComponent<Player>();
    }

    protected virtual void StartPlayer() { }


    #endregion

    #region Methods


    #endregion

    #region Coroutine


    #endregion

    #region Events


    #endregion

    #region Editor


    #endregion

}