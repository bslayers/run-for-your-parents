using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BetterSlider : Slider
{
#region Variables


#endregion

#region Accessors


#endregion


#region Built-in

    public void RecoverOldSliderValues(Slider oldSlider)
    {
        interactable = oldSlider.interactable;
        transition = oldSlider.transition;
        targetGraphic = oldSlider.targetGraphic;
        colors = oldSlider.colors;
        navigation = oldSlider.navigation;
        fillRect = oldSlider.fillRect;
        handleRect = oldSlider.handleRect;
        direction = oldSlider.direction;
        minValue = oldSlider.minValue;
        maxValue = oldSlider.maxValue;
        wholeNumbers = oldSlider.wholeNumbers;
        value = oldSlider.value;

        this.onValueChanged = oldSlider.onValueChanged;
    }

    #endregion

    #region Methods


    #endregion


    #region Events

    public override void OnMove(AxisEventData eventData)
    {
        eventData.Use();
    }

#endregion

#region Editor


#endregion

}