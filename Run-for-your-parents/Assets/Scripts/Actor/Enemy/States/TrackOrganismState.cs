using UnityEngine;

public class TrackOrganismState : TrackState
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
        DigitalizeTarget();
    }

    #endregion

    #region Methods
    /// <summary>
    /// Digitalize target : kill player and destroy object
    /// </summary>
    private void DigitalizeTarget()
    {
        InteractableManager interactableManager;
        ItemInteractable itemInteractable;
        PlayerBodyManager bodyManager = enemy.Target.GetComponent<PlayerBodyManager>();
        if (bodyManager)
        {
            bodyManager.Digitalize();
        }
        else if ((interactableManager = enemy.Target.GetComponent<InteractableManager>()) != null)
        {
            interactableManager.Digitalize();
        }
        else if ((itemInteractable = enemy.Target.GetComponent<ItemInteractable>()) != null)
        {
            itemInteractable.DestroyInteractable();
        }

        enemy.Target = null;
    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion

}