using UnityEditor;
using UnityEngine;

public abstract class InteractableObjectSO : MonoBehaviour, IInteractable
{
    #region Variables

    [Tooltip("If true, this GameObject will have an InteractionEvent component")]
    public bool useEvents;
    [Tooltip("message displayed to player when looking at an interactable")]
    public string promptMessage = "";

    [SerializeField]
    protected bool isDetector = false;

    private InteractionEvents interactionEvents;

    #endregion

    #region Accessors

    public bool IsDetector
    {
        get => isDetector;
        protected set
        {
            if (value == isDetector) { return; }
            isDetector = value;
            IsDetectorHasBeenModified();
        }
    }


    #endregion


    #region Built-in
    void Awake()
    {
        InitObject();
    }

    protected virtual void InitObject() 
    {
        interactionEvents = GetComponent<InteractionEvents>();
    }

    void Start()
    {
        StartObject();
    }

    protected virtual void StartObject() { }

    public virtual void DestroyInteractable()
    {
        Destroy(gameObject);
    }

    public void BaseInteract(GameObject actor, BodyMemberType member, Collider collider)
    {
        if (useEvents) { interactionEvents.OnInteract.Invoke(); }
        Interact(actor, member, collider);
    }

    /// <summary>
    /// Return the message of the interactable to prompt to the player
    /// </summary>
    /// <returns></returns>
    public virtual string GetMessageToPrompt()
    {
        return promptMessage;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Represent the interaction behaviour of the subclass
    /// </summary>
    /// <param name="actor">a GameObject representing the actor who interact with the interactable</param>
    /// <param name="member">the member of a player actor which interact with the interactable</param>
    /// <param name="collider">which collider of the interactable the actor had interacted</param>
    protected abstract void Interact(GameObject actor, BodyMemberType member, Collider collider);

    protected virtual void IsDetectorHasBeenModified() { }


    #endregion


    #region Events


    #endregion

    #region Editor
#if UNITY_EDITOR
    void OnValidate()
    {
        InteractableObjectOnValidate();
    }

    protected virtual void InteractableObjectOnValidate()
    {
        EditorApplication.delayCall += () =>
        {
            ValidateInteractionEvent();
        };
    }

    private void ValidateInteractionEvent()
    {
        if (this == null) { return; }
        if (useEvents)
        {
            interactionEvents = GetComponent<InteractionEvents>();
            if (interactionEvents == null) { interactionEvents = gameObject.AddComponent<InteractionEvents>(); }

            return;
        }

        if (interactionEvents != null)
        {
            DestroyImmediate(interactionEvents);
            interactionEvents = null;
        }
    }

#endif
    #endregion
}
