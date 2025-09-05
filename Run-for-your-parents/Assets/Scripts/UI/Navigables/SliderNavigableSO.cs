using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public abstract class SliderNavigableSO : InteractiveNavigableSO
{
    #region Variables

    [Header("Slider Info")]
    protected BetterSlider slider;
    [Tooltip("The movement of the slider when moving left or right")]
    [SerializeField] protected float movement;

    protected float oldValue;


    #endregion

    #region Accessors



    #endregion


    #region Built-in

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Start()
    {
        base.Start();
        slider = (BetterSlider)selectable;
        isInteractive = true;
        oldValue = slider.value;
    }


    #endregion

    #region Methods

    public override void ActionDown() { }

    public override void ActionLeft()
    {
        slider.value -= movement;
    }

    public override void ActionRight()
    {
        slider.value += movement;
    }

    public override void ActionUp() { }

    public override void Back()
    {
        slider.value = oldValue;
        base.Back();
    }

    public override void Confirm()
    {
        oldValue = slider.value;
        base.Confirm();
    }

    /// <summary>
    /// Indicate to the SliderNavigable that the slider value changed
    /// </summary>
    public abstract void SliderValueChanged();

    #endregion


    #region Events



    #endregion

    #region Editor
#if UNITY_EDITOR
    protected override void OnValidate()
    {

        EditorApplication.delayCall += () =>
        {
            CheckReplacingSlider();
        };

        base.OnValidate();
    }

    private void CheckReplacingSlider()
    {
        if (selectable == null) { return; }

        if (!(selectable is Slider || selectable is BetterSlider))
        {
            Debug.LogWarning($"{name}: {nameof(selectable)} must be a Slider");
            selectable = null;
            return;
        }

        if (selectable is BetterSlider) { return; }

        Slider oldSlider = (Slider)selectable;
        GameObject selectableObject = selectable.gameObject;

        BetterSlider tmpSlider = gameObject.AddComponent<BetterSlider>();
        tmpSlider.RecoverOldSliderValues(oldSlider);

        Undo.DestroyObjectImmediate(oldSlider);

        BetterSlider newSlider = selectableObject.AddComponent<BetterSlider>();
        newSlider.RecoverOldSliderValues(tmpSlider);

        DestroyImmediate(tmpSlider, true);
        selectable = newSlider;
    }

#endif
    #endregion

}