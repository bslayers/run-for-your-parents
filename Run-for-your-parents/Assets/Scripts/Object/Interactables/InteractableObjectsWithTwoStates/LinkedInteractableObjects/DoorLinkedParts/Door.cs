using UnityEngine;

public class Door : DoorLinkedPart
{
    #region Variables

    [Header("Colliders")]
    [SerializeField]
    private Collider frontCollider;
    [SerializeField]
    private Collider backCollider;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    #endregion

    #region Methods
    protected override void PerformFirstStateAction(Collider collider)
    {
        if (collider == backCollider)
        {
            doorManager.BlockDoor();
        }
        else if (collider == frontCollider)
        {
            doorManager.ForceDoor();
        }
        else
        {
            Debug.Log("Door encounter unknown collider: " + collider);
        }
    }

    protected override void PerformSecondStateAction(Collider collider)
    {
        doorManager.CloseDoor();
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
