using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIInputsManager))]
public class MenusManager : MonoBehaviour
{
    #region Variables

    protected UIInputsManager uIInputsManager;

    [SerializeField]
    private bool unlockCursorAtInit = false;

    [Header("Menus")]
    [Tooltip("List of all menus in the game")]
    [SerializeField] protected IndexedMenuTypeList listOfMenu = new();
    private MenuSO showedMenu;


    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        uIInputsManager = GetComponent<UIInputsManager>();
        if (!unlockCursorAtInit) { LockCursor(); }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Methods
    /// <summary>
    /// Open Menu depending of the <paramref name="type"/> of menu to open
    /// </summary>
    /// <param name="type"></param>
    public void OpenMenu(MenuData.MenuType type)
    {
        if (showedMenu != null)
        { showedMenu.CloseMenu(); }

        showedMenu = listOfMenu.list[Array.IndexOf(Enum.GetValues(typeof(MenuData.MenuType)), type)];

        //Check if the menu exists
        if (showedMenu == null) { return; }

        uIInputsManager.EnableInput();

        showedMenu.OpenMenu();
        UnlockCursor();
    }

    /// <summary>
    /// Change the current Menu showed to <paramref name="menu"/>
    /// </summary>
    /// <param name="menu">the new menu to be current</param>
    public void ChangeMenu(MenuSO menu)
    {
        if (showedMenu == null)
        {
            uIInputsManager.EnableInput();
        }
        showedMenu = menu;
    }

    /// <summary>
    /// Perform a movement in the current Menu showed
    /// </summary>
    /// <param name="direction">the direction of the movement</param>
    public void MoveInMenu(Vector2 direction)
    {
        if (showedMenu == null) return;
        if (direction == Vector2.up) { showedMenu.MoveUp(); }
        if (direction == Vector2.right) { showedMenu.MoveRight(); }
        if (direction == Vector2.down) { showedMenu.MoveDown(); }
        if (direction == Vector2.left) { showedMenu.MoveLeft(); }
    }

    /// <summary>
    /// Perform back action in the current Menu showed
    /// </summary>
    public void BackInMenu()
    {
        if (showedMenu == null) return;
        showedMenu.Back();
    }

    /// <summary>
    /// Perform confirm action in the current Menu showed
    /// </summary>
    public void ConfirmInMenu()
    {
        if (showedMenu == null) return;
        showedMenu.Confirm();
    }

    /// <summary>
    /// Indicate a Menu has been closed and return to the game
    /// </summary>
    public void MenuHasBeenClosed()
    {
        uIInputsManager.DisableInput();
        uIInputsManager.MenuClosed.Invoke();
        LockCursor();
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion


    #region Events

    #endregion

    #region Editor


    #endregion

}