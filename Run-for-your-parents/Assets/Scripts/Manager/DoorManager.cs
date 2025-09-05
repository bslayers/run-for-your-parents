using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DoorManager : InteractableManager
{
    #region Variables
    private Animator animator;
    private HandleDoor handleInteractable;
    private Door doorInteractable;

    [Tooltip("The GameObject of the handle")]
    [SerializeField]
    private GameObject handleObject;
    [Tooltip("The GameObject of the door")]
    [SerializeField]
    private GameObject doorObject;

    [Tooltip("if true, this means that the door is opened")]
    public bool opened = false;

    [Header("SoundDatas")]

    [Tooltip("Enable sound at the start of the game")]
    [SerializeField]
    private SoundData openSoundData;
    [Tooltip("The sound that will be played when the door is closed")]
    [SerializeField]
    private SoundData closeSoundData;
    [Tooltip("The sound that will be played when the door is forced")]
    [SerializeField]
    private SoundData forceSoundData;
    [Tooltip("The sound that will be played when the door is blocked")]
    [SerializeField]
    private SoundData blockSoundData;

    #endregion

    #region Accessors
    public bool Opened
    {
        set
        {
            if (value == opened) return;

            opened = value;
            UpdateState();
        }
        get
        {
            return opened;
        }
    }

    #endregion


    #region Built-in

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        handleInteractable = handleObject.GetComponent<HandleDoor>();
        doorInteractable = doorObject.GetComponent<Door>();

        UpdateState();

        if (opened) OpenDoor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

    #region Methods
    /// <summary>
    /// Start open behaviour
    /// </summary>
    public void OpenDoor()
    {
        Opened = true;
        animator.SetBool("isForce", false);
        animator.SetBool("isOpened", true);

        if (!sound) return;

        SoundEmitor.EmiteSound(gameObject, openSoundData, false);

    }

    public void CloseDoor()
    {
        Opened = false;
        animator.SetBool("isForce", false);
        animator.SetBool("isOpened", false);

        if (!sound) return;

        SoundEmitor.EmiteSound(gameObject, closeSoundData, false);
    }

    /// <summary>
    /// Start force opening behaviour
    /// </summary>
    public void ForceDoor()
    {
        Opened = true;
        animator.SetBool("isForce", true);
        animator.SetBool("isOpened", true);

        if (!sound) return;

        SoundEmitor.EmiteSound(gameObject, forceSoundData, false);
    }

    /// <summary>
    /// Start blocking opening behaviour
    /// </summary>
    public void BlockDoor()
    {
        animator.SetTrigger("isBlock");

        if (!sound) return;

        SoundEmitor.EmiteSound(gameObject, blockSoundData, false);

    }

    private void UpdateState()
    {
        InteractableObjectWithTwoStatesSO.InteractableState state = opened ? InteractableObjectWithTwoStatesSO.InteractableState.State2 : InteractableObjectWithTwoStatesSO.InteractableState.State1;
        handleInteractable.CurrentState = state;
        doorInteractable.CurrentState = state;
    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
