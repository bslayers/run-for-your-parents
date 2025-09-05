using UnityEngine;

public class TrackSoundState : TrackState
{
    #region Variables


    #endregion

    #region Accessors


    #endregion


    #region Built-in
    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void StealBehaviour()
    {
        DigitalizeSound();
    }

    #endregion

    #region Methods
    /// <summary>
    /// Digitalize sound emition from target
    /// </summary>
    private void DigitalizeSound()
    {
        InteractableManager interactableManager;
        ItemInteractable itemInteractable;
        PlayerBodyManager bodyManager = enemy.Target.GetComponent<PlayerBodyManager>();

        if (bodyManager)
        {
            bodyManager.SetExisting(BodyMemberType.Voice, false);
        }
        else if ((interactableManager = enemy.Target.GetComponent<InteractableManager>()) != null)
        {
            interactableManager.Sound = false;
        }
        else if ((itemInteractable = enemy.Target.GetComponent<ItemInteractable>()) != null)
        {
            itemInteractable.Sound = false;
        }

        enemy.Target = null;

    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion

}