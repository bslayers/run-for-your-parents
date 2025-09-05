using UnityEngine.InputSystem;

public class OnFootInputHandler
{
    #region Variables
    private readonly InputActionMap actionMap;

    #endregion

    #region Accessors
    public InputAction Movement { get; private set; }
    public InputAction Look { get; private set; }
    public InputAction Jump { get; private set; }
    public InputAction Crouch { get; private set; }
    public InputAction Sprint { get; private set; }
    public InputAction UseRightHand { get; private set; }
    public InputAction UseLeftHand { get; private set; }
    public InputAction InteractRightHand { get; private set; }
    public InputAction InteractLeftHand { get; private set; }
    public InputAction Menu { get; private set; }

    public InputAction[] InventorySlots { get; private set; }

    #endregion


    #region Built-in

    public OnFootInputHandler(InputActionMap onFootMap)
    {
        actionMap = onFootMap;

        Movement = actionMap.FindAction("Movement");
        Look = actionMap.FindAction("Look");
        Jump = actionMap.FindAction("Jump");
        Crouch = actionMap.FindAction("Crouch");
        Sprint = actionMap.FindAction("Sprint");
        UseRightHand = actionMap.FindAction("UseRightHand");
        UseLeftHand = actionMap.FindAction("UseLeftHand");
        InteractRightHand = actionMap.FindAction("InteractRightHand");
        InteractLeftHand = actionMap.FindAction("InteractLeftHand");
        Menu = actionMap.FindAction("Menu");

        InventorySlots = new InputAction[]
        {
            actionMap.FindAction("Inventory1"),
            actionMap.FindAction("Inventory2"),
            actionMap.FindAction("Inventory3"),
            actionMap.FindAction("Inventory4"),
            actionMap.FindAction("Inventory5"),
            actionMap.FindAction("Inventory6"),
        };
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