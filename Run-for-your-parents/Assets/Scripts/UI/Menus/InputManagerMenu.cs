using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerMenu : MenuSO
{

    #region Variables

    #endregion

    #region Accessors


    #endregion


    #region Built-in


    public void OpenInputManager()
    {
        OpenMenu();
        menusManager.ChangeMenu(this);
    }

    public void SaveBindingOverride()
    {
        string rebinds = InputActionManager.Instance.inputAction.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
        PlayerPrefs.Save();
    }

    public void OnRebindComplete()
    {
        SaveBindingOverride();

        PlayerInputsManager playerInputsManager = FindAnyObjectByType<PlayerInputsManager>();
        playerInputsManager.LoadBindingOverrides();
    }



    #endregion

    #region Methods


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion

}