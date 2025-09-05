using UnityEngine;

public class GameOverMenu : MenuSO
{
    #region Variables

    [Tooltip("The statistic manager to update the statistics")]
    [SerializeField] private StatisticManager statisticManager;

    #endregion

    #region Accessors


    #endregion


    #region Built-in
    // Update is called once per frame
    void Update()
    {

    }

    public override void OpenSubMenu()
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region Methods

    public void ReturnToLandfill()
    {
        Game.Instance.ChangeScene("Landfill");
    }

    public void Restart()
    {
        Game.Instance.RestartScene();
    }

    public void ReturnToMainMenu()
    {
        Game.Instance.ChangeScene("MainMenu");
    }




    #endregion


    #region Events

    void OnEnable()
    {
        statisticManager.UpdateStatistics();
    }


    #endregion

    #region Editor


    #endregion
}