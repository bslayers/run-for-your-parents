using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(OnMoveFirewall))]
public abstract class MenuSO : MonoBehaviour
{
    #region Variables
    protected enum Action : sbyte { Show, Hide };

    [Header("For Initialization")]
    [RequiredReference]
    [SerializeField] protected MenusManager menusManager;

    [Tooltip("Resource having all necessary information for the menu")]
    [RequiredReference]
    [SerializeField] private MenuData menuData;
    protected MenuSO menu;

    [Tooltip("Let None if the GameObject to hide is the gameObject of the menu")]
    [SerializeField] protected GameObject thingToHideDuringMenuChanging;

    [Tooltip("The Navigable hoving by default when a moving action is performed and no Navigable is hoved")]
    [RequiredReference]
    [SerializeField] protected NavigableSO defaultNavigable;

    [Header("For menuing")]
    [SerializeField] protected MenuSO superMenu;
    [SerializeField] protected bool cantExitMenu = false;
    [ContextMenuItem("Recover Navigables", nameof(RecoverAllSubNavigable))]
    [Tooltip("Put all Navigable of the menu for update their colors at initializing")]
    [SerializeField] protected NavigableSO[] navigables = { };
    [Tooltip("Keep the current Navigable during the menu changing")]
    [SerializeField] protected bool keepCurrentNavigableDuringChangingMenu = false;
    [Tooltip("if true, the time is stopped when the menu is opened")]
    [SerializeField] protected bool timeControl = true;


    private NavigableSO currentNavigable;
    protected bool selectingNavigable = false;

    protected bool showed = false;

    #endregion

    #region Accessors

    protected NavigableSO CurrentNavigable
    {
        get => currentNavigable;
        set
        {
            if (value == currentNavigable) { return; }
            currentNavigable?.Dehover();
            currentNavigable = value;
            currentNavigable?.Hover();
        }
    }

    protected bool SelectingNavigable
    {
        get => selectingNavigable;
        set
        {
            selectingNavigable = value && CurrentNavigable != null;
        }
    }

    #endregion

    #region Built-in
    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        UpdateAllNavigable();
        InitListener();
    }

    void Start()
    {
        StartMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void StartMenu()
    {
        menu = this;
        menu.gameObject.SetActive(showed);
        if (!thingToHideDuringMenuChanging) { thingToHideDuringMenuChanging = menu.gameObject; }

    }

    protected virtual void InitListener()
    {
        foreach (NavigableSO nav in navigables)
        {
            nav.IsHovering.AddListener(OnNavigableIsHovering);
            nav.IsDehovered.AddListener(OnNavigableIsDehovered);
        }
    }

    #endregion

    #region Methods

    protected void RecoverAllSubNavigable()
    {
        navigables = GetComponentsInChildren<NavigableSO>();
#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(this, "Update Navigables via ContextMenu");
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    /// <summary>
    /// Open the Menu
    /// </summary>
    public void OpenMenu()
    {
        OpenCloseMenu(menu.gameObject, Action.Show, timeControl);
    }

    /// <summary>
    /// Open or close the <paramref name="chosenMenu"/> depending of the variable showed
    /// </summary>
    /// <param name="chosenMenu">the menu to open or close</param>
    /// <param name="action">The action to perform</param>
    /// <param name="timeControl">Control the time or not</param>
    protected void OpenCloseMenu(GameObject chosenMenu, Action action, bool timeControl)
    {
        showed = action == Action.Show;
        chosenMenu.SetActive(showed);

        if (showed && !keepCurrentNavigableDuringChangingMenu)
        {
            CurrentNavigable = null;
        }

        if (timeControl) Time.timeScale = showed ? 0 : 1;
    }

    /// <summary>
    /// Close the menu
    /// </summary>
    public void CloseMenu()
    {
        OpenCloseMenu(menu.gameObject, Action.Hide, timeControl);
    }

    /// <summary>
    /// Hide the menu for let a sub-menu opening
    /// </summary>
    public virtual void OpenSubMenu()
    {
        OpenCloseMenu(thingToHideDuringMenuChanging, Action.Hide, false);
    }

    /// <summary>
    /// Close the menu and show his superMenu.
    /// </summary>
    public virtual void CloseMenuAndOpenSuperMenu()
    {
        OpenCloseMenu(thingToHideDuringMenuChanging, Action.Hide, false);
        CurrentNavigable = null;
        superMenu.SubMenuHasBeenClosed();
    }

    /// <summary>
    /// Indicate to this menu that one his sub-menu has been closed
    /// </summary>
    public virtual void SubMenuHasBeenClosed()
    {
        OpenCloseMenu(thingToHideDuringMenuChanging, Action.Show, false);
        menusManager.ChangeMenu(this);
        if (CurrentNavigable != null) { CurrentNavigable.Hover(); }
    }

    /// <summary>
    /// Close the menu and indicate to the MenusManager returning to the game
    /// </summary>
    protected void CloseMenuAndReturnToGame()
    {
        CloseMenu();
        CurrentNavigable = null;
        menusManager.MenuHasBeenClosed();
    }


    /// <summary>
    /// Move to up Navigable from the current Navigable, or hover the default Navigable otherwise.
    /// </summary>
    public void MoveUp()
    {
        if (SelectingNavigable)
        {
            ((InteractiveNavigableSO)CurrentNavigable).ActionUp();
            return;
        }
        if (HasCurrentNavigable())
        { CurrentNavigable = CurrentNavigable.MoveUp(); }
    }

    /// <summary>
    /// Move to right Navigable from the current Navigable, or hover the default Navigable otherwise.
    /// </summary>
    public void MoveRight()
    {
        if (SelectingNavigable)
        {
            ((InteractiveNavigableSO)CurrentNavigable).ActionRight();
            return;
        }
        if (HasCurrentNavigable())
        { CurrentNavigable = CurrentNavigable.MoveRight(); }
    }

    /// <summary>
    /// Move to down Navigable from the current Navigable, or hover the default Navigable otherwise.
    /// </summary>
    public void MoveDown()
    {
        if (SelectingNavigable)
        {
            ((InteractiveNavigableSO)CurrentNavigable).ActionDown();
            return;
        }
        if (HasCurrentNavigable())
        { CurrentNavigable = CurrentNavigable.MoveDown(); }
    }

    /// <summary>
    /// Move to left Navigable from the current Navigable, or hover the default Navigable otherwise.
    /// </summary>
    public void MoveLeft()
    {
        if (SelectingNavigable)
        {
            ((InteractiveNavigableSO)CurrentNavigable).ActionLeft();
            return;
        }
        if (HasCurrentNavigable())
        { CurrentNavigable = CurrentNavigable.MoveLeft(); }
    }

    /// <summary>
    /// Throw the Confirm action of the current Navigable
    /// </summary>
    public void Confirm()
    {
        if (CurrentNavigable == null) { return; }

        if (SelectingNavigable)
        {
            ((InteractiveNavigableSO)CurrentNavigable).Confirm();
            SelectingNavigable = false;
            return;
        }

        if (CurrentNavigable.IsInteractive) { SelectingNavigable = !SelectingNavigable; }
        CurrentNavigable.Select();
    }

    /// <summary>
    /// Throw the Back action of the current Navigable
    /// </summary>
    public void Back()
    {
        if (SelectingNavigable)
        {
            ((InteractiveNavigableSO)CurrentNavigable).Back();
            SelectingNavigable = false;
            return;
        }

        if (cantExitMenu) { return; }

        if (superMenu)
        {
            CloseMenuAndOpenSuperMenu();
        }
        else
        {
            CloseMenuAndReturnToGame();
        }
    }

    protected bool HasCurrentNavigable()
    {
        if (CurrentNavigable == null)
        {
            CurrentNavigable = defaultNavigable;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Update colors of every Navigable in navigables
    /// </summary>
    protected void UpdateAllNavigable()
    {
        if (menuData == null) { return; }
        foreach (NavigableSO navigable in navigables)
        {
            navigable.UpdateColor(menuData.colorDataOfNavigables);
        }
    }

    #endregion

    #region Events
    protected void OnNavigableIsHovering(NavigableSO hoveredNavigable)
    {
        if (SelectingNavigable)
        {
            SelectingNavigable = false;
            if (CurrentNavigable)
            {
                ((InteractiveNavigableSO)CurrentNavigable).Back();
                CurrentNavigable.Deselect();
            }
        }

        CurrentNavigable = hoveredNavigable;

    }

    protected void OnNavigableIsDehovered(NavigableSO navigable)
    {
        if (navigable != CurrentNavigable) { return; }
        currentNavigable = null;
    }

    #endregion

    #region Editor
#if UNITY_EDITOR

    private void OnValidate()
    {
        if (!thingToHideDuringMenuChanging) { thingToHideDuringMenuChanging = gameObject; }

        if (superMenu) { cantExitMenu = false; timeControl = false; }
    }

    private void OnDestroy()
    {
        if (EditorApplication.isPlaying) { return; }
        EditorApplication.delayCall += () =>
        {
            Undo.DestroyObjectImmediate(GetComponent<OnMoveFirewall>());
        };
    }

#endif
    #endregion
}
