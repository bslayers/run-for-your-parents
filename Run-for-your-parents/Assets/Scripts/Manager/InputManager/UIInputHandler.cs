using UnityEngine.InputSystem;

public class UIInputHandler
{
    #region Variables
    private readonly InputActionMap actionMap;

    #endregion

    #region Accessors
    public InputAction Navigate { get; private set; }
    public InputAction Submit { get; private set; }
    public InputAction Cancel { get; private set; }

    #endregion

    #region Built-in

    public UIInputHandler(InputActionMap onUIMap)
    {
        actionMap = onUIMap;

        Navigate = actionMap.FindAction("Navigate");
        Submit = actionMap.FindAction("Submit");
        Cancel = actionMap.FindAction("Cancel");
    }

    #endregion

    #region Methods
    public void Enable() => actionMap.Enable();
    public void Disable() => actionMap.Disable();

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion

}