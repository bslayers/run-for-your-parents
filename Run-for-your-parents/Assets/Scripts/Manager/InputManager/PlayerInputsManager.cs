using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsManager : InputsManagerSO
{
    #region Variables
    public OnFootInputHandler onFoot;
    private Player player;
    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerItemManager itemManager;
    private PlayerAnimatorManager animatorManager;
    private PlayerBodyManager bodyManager;

    private System.Action<InputAction.CallbackContext>[] slotCallbacks;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    void Update()
    {
        if (onFoot != null)
        {
            CheckOnFootInputs();
        }
    }

    void FixedUpdate()
    {
        if (motor != null && onFoot != null)
        {
            motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        }
    }

    private void LateUpdate()
    {
        if (look != null && onFoot != null)
        {
            look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
        }
    }

    protected override void StartInputsManager()
    {
        player = GetComponent<Player>();
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        itemManager = GetComponent<PlayerItemManager>();
        animatorManager = player.animatorManager;
        bodyManager = GetComponent<PlayerBodyManager>();

        var onFootMap = InputActionManager.Instance.inputAction.FindActionMap("OnFoot");
        onFoot = new OnFootInputHandler(onFootMap);

        player.uIInputs.MenuClosed.AddListener(OnUIInputsManagerMenuClose);

        onFoot.Jump.performed += OnJump;
        onFoot.Crouch.performed += OnSneek;
        onFoot.Crouch.canceled += OnSneek;
        onFoot.Sprint.performed += OnRun;
        onFoot.Sprint.canceled += OnRun;

        slotCallbacks = new System.Action<InputAction.CallbackContext>[onFoot.InventorySlots.Length];

        for (int i = 0; i < onFoot.InventorySlots.Length; i++)
        {
            int slotIndex = i;
            slotCallbacks[i] = ctx => OnInventorySlot(ctx, slotIndex);
            onFoot.InventorySlots[i].performed += slotCallbacks[i];
        }

        LoadBindingOverrides();

        base.StartInputsManager();
    }

    #endregion

    #region Methods


    //TODO
    /// <summary>
    /// 
    /// </summary>
    protected void CheckOnFootInputs()
    {
        if (onFoot.Menu?.triggered == true)
        {
            OpenMenu(MenuData.MenuType.Pause);
        }

        //Use Item
        if (onFoot.UseRightHand?.triggered == true && bodyManager.GetExisting(BodyMemberType.RightHand)) { itemManager?.ActionItemInInventory(BodyMemberType.RightHand, ItemData.USING_TYPE.ThrowingItem); }
        if (onFoot.UseLeftHand?.triggered == true && bodyManager.GetExisting(BodyMemberType.LeftHand)) { itemManager?.ActionItemInInventory(BodyMemberType.LeftHand, ItemData.USING_TYPE.ThrowingItem); }
    }

    protected void OpenMenu(MenuData.MenuType type)
    {
        player.uIInputs.OpenMenu(type);
        DisableInput();
    }

    /// <summary>
    /// Enter menu behaviour for InputsManager
    /// </summary>
    public override void DisableInput() => onFoot?.Disable();

    /// <summary>
    /// Exit menu behaviour for InputsManager
    /// </summary>
    public override void EnableInput() => onFoot?.Enable();

    public void LoadBindingOverrides()
    {
        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);
        if (!string.IsNullOrEmpty(rebinds))
        {
            InputActionManager.Instance.inputAction.LoadBindingOverridesFromJson(rebinds);
        }
    }

    #endregion


    #region Events
    private void OnEnable()
    {
        EnableInput();
    }

    private void OnDisable()
    {
        DisableInput();
    }

    void OnDestroy()
    {
        onFoot.Jump.performed -= OnJump;
        onFoot.Crouch.performed -= OnSneek;
        onFoot.Crouch.canceled -= OnSneek;
        onFoot.Sprint.performed -= OnRun;
        onFoot.Sprint.canceled -= OnRun;

        if (slotCallbacks != null)
        {
            for (int i = 0; i < slotCallbacks.Length; i++)
            {
                onFoot.InventorySlots[i].performed -= slotCallbacks[i];
            }
        }
    }

    private void OnUIInputsManagerMenuClose() => EnableInput();

    internal void EnableActionMap(string v)
    {
        InputActionManager.Instance.inputAction.FindActionMap(v)?.Enable();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        motor.Jump();
    }

    private void OnSneek(InputAction.CallbackContext context)
    {
        motor.Crouch();
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        motor.Sprint();
    }

    private void OnInventorySlot(InputAction.CallbackContext context, int slotIndex)
    {
        itemManager.SelectedItem(slotIndex);
    }


    #endregion

    #region Editor


    #endregion
}
