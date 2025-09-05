using UnityEngine;

public class EndMenu : MenuSO
{
    #region Variables


    #endregion

    #region Accessors


    #endregion


    #region Built-in

    public void ReturnToMainMenu()
    {
        Game.Instance.ChangeScene("MainMenu");
    }

    public void RestartLevel()
    {
        Game.Instance.RestartScene();
    }

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