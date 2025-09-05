using UnityEngine;
using TMPro;

public class PlayerUI : PlayerComponentSO
{
    #region Variables
    private PlayerInputsManager inputsManager;

    [Tooltip("The text to display the prompt message")]
    [SerializeField]
    private TextMeshProUGUI promptText;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    void Update()
    {

    }

    protected override void InitPlayer()
    {
        base.InitPlayer();
        inputsManager = player.playerInputs;
    }

    #endregion

    #region Methods
    /// <summary>
    /// Update the interaction text with <paramref name="promptMessage"/> into
    /// </summary>
    /// <param name="promptMessage">the message to prompt</param>
    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    //TODO
    /// <summary>
    /// 
    /// </summary>
    public void MenuHasBeenClosed()
    {
        inputsManager.EnableInput();
    }

    //TODO
    /// <summary>
    /// 
    /// </summary>
    public void ChangeScene()
    {
        inputsManager.EnableInput();
        Time.timeScale = 1;
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
