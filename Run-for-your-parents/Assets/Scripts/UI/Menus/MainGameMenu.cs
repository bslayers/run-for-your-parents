using UnityEngine;

public class MainGameMenu : MenuSO
{
    #region Variables


    #endregion

    #region Accessors


    #endregion


    #region Built-in
    protected override void StartMenu()
    {
        showed = true;
        base.StartMenu();
        menusManager.ChangeMenu(this);
    }

    #endregion

    #region Methods
    /// <summary>
    /// Play game button behaviour
    /// </summary>
    public void PlayGame(string sceneName)
    {
        Game.Instance.ChangeScene(sceneName);
    }

    /// <summary>
    /// Quit Game button behaviour
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
