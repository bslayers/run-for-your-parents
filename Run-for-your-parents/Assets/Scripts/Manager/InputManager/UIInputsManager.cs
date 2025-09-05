using UnityEngine;
using UnityEngine.Events;

public class UIInputsManager : InputsManagerSO
{
    #region Variables
    private MenusManager menusManager;
    private UIInputHandler onUI;

    #endregion

    #region Accessors


    #endregion


    #region Built-in 

    // Update is called once per frame
    void Update()
    {
        if (onUI != null)
        {
            CheckOnUIInputs();
        }
    }

    protected override void StartInputsManager()
    {
        menusManager = GetComponent<MenusManager>();

        var onUIMap = InputActionManager.Instance.inputAction.FindActionMap("OnUI");
        onUI = new UIInputHandler(onUIMap);

        base.StartInputsManager();
    }

    public override void DisableInput() => onUI?.Disable();

    public override void EnableInput() => onUI?.Enable();



    #endregion

    #region Methods
    /// <summary>
    /// Check inputs from PlayerInputs.OnUI for performing actions in UI
    /// </summary>
    protected void CheckOnUIInputs()
    {
        if (onUI == null || menusManager == null) return;
        if (onUI.Navigate?.triggered == true)
        {
            menusManager.MoveInMenu(onUI.Navigate.ReadValue<Vector2>());
        }
        if (onUI.Cancel?.triggered == true)
        {
            menusManager.BackInMenu();
        }
        if (onUI.Submit?.triggered == true)
        {
            menusManager.ConfirmInMenu();
        }
    }

    /// <summary>
    /// Open menu depending of the <paramref name="type"/> of menu to open
    /// </summary>
    /// <param name="type"></param>
    public void OpenMenu(MenuData.MenuType type)
    {
        menusManager.OpenMenu(type);
    }


    #endregion


    #region Events
    [HideInInspector]
    public UnityEvent MenuClosed;

    private void OnDisable() => DisableInput();

    #endregion

    #region Editor


    #endregion

}