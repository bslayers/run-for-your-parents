using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuSO
{
    #region Variables
    [Tooltip("The game object of the buttons")]
    [SerializeField] private GameObject buttons;

    #endregion

    #region Accessors


    #endregion


    #region Built-in
    //TODO
    /// <summary>
    /// 
    /// </summary>
    protected override void StartMenu()
    {
        base.StartMenu();
    }


    #endregion

    #region Methods
    /// <summary>
    /// Resume button behaviour
    /// </summary>
    public void OnClickResume()
    {
        CloseMenu();
        menusManager.MenuHasBeenClosed();
    }

    /// <summary>
    /// Restart sccene button behaviour
    /// </summary>
    public void OnClickRestart()
    {
        Game.Instance.RestartScene();
    }

    //TODO
    /// <summary>
    /// 
    /// </summary>
    public void OnClickAbandon()
    {
        Game.Instance.ChangeScene("Landfill");
    }

    /// <summary>
    /// Quit button behaviour
    /// </summary>
    public void OnClickQuit()
    {
        Game.Instance.ChangeScene(0);
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
