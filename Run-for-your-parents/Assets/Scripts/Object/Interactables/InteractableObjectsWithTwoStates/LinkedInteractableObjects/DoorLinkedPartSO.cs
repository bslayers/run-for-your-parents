using UnityEngine;

public abstract class DoorLinkedPart : LinkedInteractableObjectSO
{
    #region Variables
    protected DoorManager doorManager;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    protected override void InitObject()
    {
        base.InitObject();
        doorManager = (DoorManager)interactableManager;
    }

    #endregion

    #region Methods


    #endregion


    #region Events


    #endregion

    #region Editor
#if UNITY_EDITOR
    protected override void InteractableObjectOnValidate()
    {
        base.InteractableObjectOnValidate();

        if (interactableManager == null) { return; }
        if (!(interactableManager is DoorManager))
        {
            Debug.LogWarning($"{name}: {nameof(interactableManager)} must be a DoorManager");
            interactableManager = null;
            return;
        }
        doorManager = (DoorManager)interactableManager;

    }

#endif
    #endregion

}