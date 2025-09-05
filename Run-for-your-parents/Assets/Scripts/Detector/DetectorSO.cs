using System.Collections;
using UnityEngine;

public abstract class DetectorSO : MonoBehaviour
{
    #region Variables

    [SerializeField]
    protected bool isActive = true;

    [SerializeField]
    protected AudioSource digitalizeSound;

    #endregion

    #region Accessors

    public bool IsActive { 
        get => isActive; 
        protected set 
        {
            if (value == isActive) { return; }
            isActive = value;
            IsActiveUpdated();
        } 
    }

    #endregion


    #region Built-in




    #endregion

    #region Methods

    protected virtual void DigitalizePlayer(PlayerBodyManager player, Collider other)
    {
        digitalizeSound.Play();
    }

    protected virtual void DigitalizeItem(ItemInteractable item)
    {
        StartCoroutine(DelayDigitalizing(item));
        return;
    }



    protected virtual void DigitalizeInteractable(InteractableManager interactable)
    {
        StartCoroutine(DelayDigitalizing(interactable));
        return;
    }

    private void Digitalize(Collider other)
    {
        DigitalizeBehaviour(other);
    }

    protected virtual void DigitalizeBehaviour(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DigitalizePlayer(other.GetComponent<PlayerBodyManager>(), other);
            return;
        }
        else if (other.CompareTag("Object"))
        {
            DigitalizeInteractable(other.GetComponent<InteractableManager>());
            return;
        }
        else if (other.CompareTag("Item"))
        {
            DigitalizeItem(other.GetComponent<ItemInteractable>());
            return;
        }
    }

    protected virtual void IsActiveUpdated() { }

    #endregion


    #region Events

    protected bool IsValideCollider(Collider collider)
    {
        return isActive && !collider.isTrigger;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!IsValideCollider(collision.collider)) { return; }

        OnCollisionEnterBehaviour(collision.collider);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!IsValideCollider(other)) { return; }
        OnCollisionEnterBehaviour(other);
    }

    protected virtual void OnCollisionEnterBehaviour(Collider collider)
    {
        Digitalize(collider);
    }

    void OnCollisionExit(Collision collision)
    {
        if (!IsValideCollider(collision.collider)) { return; }
        OnCollisionExitBehaviour(collision.collider);
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsValideCollider(other)) { return; }
        OnCollisionExitBehaviour(other);
    }

    protected virtual void OnCollisionExitBehaviour(Collider collider)
    {

    }


    #endregion

    #region Coroutines

    IEnumerator DelayDigitalizing(InteractableManager interactable)
    {
        yield return DelayDigitalizing();
        interactable.Digitalize();
    }

    IEnumerator DelayDigitalizing(ItemInteractable item)
    {
        yield return DelayDigitalizing();
        item.DestroyInteractable();
    }

    IEnumerator DelayDigitalizing()
    {
        digitalizeSound.Play();
        yield return new WaitForSeconds(.5f);
    }

    #endregion

    #region Editor

    #endregion

}