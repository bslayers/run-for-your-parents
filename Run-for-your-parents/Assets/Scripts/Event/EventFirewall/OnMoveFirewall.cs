using UnityEngine;
using UnityEngine.EventSystems;

public class OnMoveFirewall : MonoBehaviour, IMoveHandler
{

public void OnMove(AxisEventData eventData) 
    {
        eventData.Use();
    }

}