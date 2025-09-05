using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public abstract class NavigableSO : UIBehaviour
{
    #region Variables

    [Tooltip("The selectable component of the Navigable")]
    [SerializeField]
    [RequiredReference]
    protected Selectable selectable;

    [SerializeField, HideInInspector]
    private Selectable e_selectable;

    protected bool isInteractive = false;

    [Header("For navigation")]
    [Tooltip("The Navigable located at the top")]
    [SerializeField]
    protected NavigableSO toUp;
    [Tooltip("The Navigable located at the right")]
    [SerializeField]
    protected NavigableSO toRight;
    [Tooltip("The Navigable located at the bottom")]
    [SerializeField]
    protected NavigableSO toDown;
    [Tooltip("The Navigable located at the left")]
    [SerializeField]
    protected NavigableSO toLeft;


    #endregion

    #region Accessors

    public bool IsInteractive { get => isInteractive; }

    #endregion


    #region Built-in
    protected override void Start()
    {
        base.Start();

        selectable.navigation = new Navigation { mode = Navigation.Mode.None };
    }

    #endregion

    #region Methods
    /// <summary>
    /// Select the Navigable
    /// </summary>
    public virtual void Select()
    {
        selectable.Select();
    }

    /// <summary>
    /// Deselect the Navigable
    /// </summary>
    public virtual void Deselect()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// Hover the Navigable
    /// </summary>
    public virtual void Hover()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pointerDrag = gameObject;
        ExecuteEvents.Execute(selectable.gameObject, eventData, ExecuteEvents.pointerEnterHandler);
        IsHovering.Invoke(this);
    }

    /// <summary>
    /// dehover the Navigable
    /// </summary>
    public virtual void Dehover()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pointerDrag = gameObject;
        ExecuteEvents.Execute(selectable.gameObject, eventData, ExecuteEvents.pointerExitHandler);
        IsDehovered.Invoke(this);
    }

    /// <summary>
    /// Return the Navigable located at the top (toUp)
    /// </summary>
    public virtual NavigableSO MoveUp()
    {
        return toUp == null ? this : toUp;
    }

    /// <summary>
    /// Return the Navigable located at the right (toRight)
    /// </summary>
    public virtual NavigableSO MoveRight()
    {
        return toRight == null ? this : toRight;
    }

    /// <summary>
    /// Return the Navigable located at the bottom (toDown)
    /// </summary>
    public virtual NavigableSO MoveDown()
    {
        return toDown == null ? this : toDown;
    }

    /// <summary>
    /// Return the Navigable located at the left (toLeft)
    /// </summary>
    public virtual NavigableSO MoveLeft()
    {
        return toLeft == null ? this : toLeft;
    }

    /// <summary>
    /// Update colors of the Navigable
    /// </summary>
    /// <param name="colors"></param>
    public virtual void UpdateColor(NavigableColorData colors)
    {
        if (!selectable)
        {
            Debug.LogWarning($"{name}: has no selectable");
            return;
        }
        ColorBlock cb = selectable.colors;
        cb.pressedColor = colors.pressedColor;
        cb.selectedColor = colors.SelectedColor;
        cb.normalColor = colors.normalColor;
        cb.highlightedColor = colors.highlightedColor;

        selectable.colors = cb;
    }


    #endregion


    #region Events
    [HideInInspector]
    public UnityEvent<NavigableSO> IsHovering;
    [HideInInspector]
    public UnityEvent<NavigableSO> IsDehovered;


    #endregion

    #region Editor
#if UNITY_EDITOR

    protected override void OnDestroy()
    {
        if (EditorApplication.isPlaying) { return; }
        if (selectable == null) { return; }
        EditorApplication.delayCall += () =>
        {
            DestroyPointerFirewall();
        };
        base.OnDestroy();
    }

    protected override void OnValidate()
    {
        EditorApplication.delayCall += () =>
        {
            CheckFirewallBeing();
        };

        base.OnValidate();
    }

    private void CheckFirewallBeing()
    {
        if (selectable && !selectable.GetComponent<OnPointerFirewall>()) { AddPointerFirewall(); }

        if (selectable == e_selectable) { return; }

        if (!selectable && e_selectable)
        {
            DestroyPointerFirewall();
            e_selectable = null;
            return;
        }
        if (selectable)
        {
            if (e_selectable) { DestroyPointerFirewall(); }
            AddPointerFirewall();
            e_selectable = selectable;
        }
    }

    private void AddPointerFirewall()
    {
        if (selectable.GetComponent<OnPointerFirewall>()) { return; }
        selectable.gameObject.AddComponent<OnPointerFirewall>().navigable = this;
    }

    private void DestroyPointerFirewall()
    {
        OnPointerFirewall toDestroy = e_selectable.GetComponent<OnPointerFirewall>();
        if (!toDestroy) { return; }
        Undo.DestroyObjectImmediate(toDestroy);
    }

#endif
    #endregion

}