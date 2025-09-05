using UnityEngine;

public class PlayerInteract : PlayerComponentSO
{
    #region Variables
    private Camera cam;
    [Tooltip("The distance at which the player can interact with an object")]
    [SerializeField]
    private float distance = 3f;
    [Tooltip("The layer mask that the player can interact with")]
    [SerializeField]
    private LayerMask mask;
    private PlayerUI playerUI;
    private PlayerInputsManager inputsManager;
    private PlayerBodyManager bodyManager;
    private PlayerItemManager itemManager;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty);

        CheckInteraction();
    }

    protected override void InitPlayer()
    {
        base.InitPlayer();
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputsManager = GetComponent<PlayerInputsManager>();
        bodyManager = GetComponent<PlayerBodyManager>();
        itemManager = GetComponent<PlayerItemManager>();
    }

    #endregion

    #region Methods

    private void CheckInteraction()
    {
        //create a ray cast at the center of the camera, shooting outwards
        Ray ray = new(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        //variable to store our collision infomation

        if (Physics.Raycast(ray, out RaycastHit hitInfo, distance, mask))
        {
            InteractableObjectSO interactable = hitInfo.collider.GetComponent<InteractableObjectSO>();
            if (interactable == null) return;

            playerUI.UpdateText(interactable.GetMessageToPrompt());
            
            InteractWithCastedInteractable(interactable, hitInfo);
        }
        else
        {
            if (inputsManager.onFoot?.InteractRightHand?.triggered == true && bodyManager.GetExisting(BodyMemberType.RightHand)) { itemManager?.ActionItemInInventory(BodyMemberType.RightHand, ItemData.USING_TYPE.UsingItem); }
            if (inputsManager.onFoot?.InteractLeftHand?.triggered == true && bodyManager.GetExisting(BodyMemberType.LeftHand)) { itemManager?.ActionItemInInventory(BodyMemberType.LeftHand, ItemData.USING_TYPE.UsingItem); }
        }
    }

    private void InteractWithCastedInteractable(InteractableObjectSO interactable, RaycastHit hitInfo)
    {
        if (inputsManager.onFoot.InteractRightHand.triggered)
        {
            if (bodyManager.GetExisting(BodyMemberType.RightHand)) { InteractWithMember(BodyMemberType.RightHand, interactable, hitInfo); }
            else if (bodyManager.GetExisting(BodyMemberType.RightArm)) { InteractWithMember(BodyMemberType.RightArm, interactable, hitInfo); }
        }
        else if (inputsManager.onFoot.InteractLeftHand.triggered)
        {
            if (bodyManager.GetExisting(BodyMemberType.LeftHand)) { InteractWithMember(BodyMemberType.LeftHand, interactable, hitInfo); }
            else if (bodyManager.GetExisting(BodyMemberType.LeftArm)) { InteractWithMember(BodyMemberType.LeftArm, interactable, hitInfo); }
        }
    }

    private void InteractWithMember(BodyMemberType member, InteractableObjectSO interactable, RaycastHit hitInfo)
    {
        interactable.BaseInteract(gameObject, member, hitInfo.collider);
        if (interactable.IsDetector) { bodyManager.SetExisting(member, false); }
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
