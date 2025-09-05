using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class OnPointerFirewall : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("The navigable object that this firewall is attached to.")]
    public NavigableSO navigable;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == navigable.gameObject)
        {
            return;
        }
        navigable.Hover();
        eventData.Use();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == navigable.gameObject)
        {
            return;
        }
        navigable.Dehover();
        eventData.Use();
    }
}