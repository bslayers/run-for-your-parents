using UnityEngine;

public abstract class InteractableObjectWithTwoStatesSO : InteractableObjectSO
{
    #region Variables
    public enum InteractableState { State1, State2 };


    [SerializeField]
    protected string firstStatePromptMessage = "";
    [SerializeField]
    protected string secondStatePromptMessage = "";


    protected InteractableState currentState;


    #endregion

    #region Accessors

    public InteractableState CurrentState
    {
        set => currentState = value;
    }

    #endregion


    #region Built-in


    #endregion

    #region Methods
    public override string GetMessageToPrompt()
    {
        return currentState == InteractableState.State1 ? firstStatePromptMessage : secondStatePromptMessage;
    }

    protected override void Interact(GameObject actor, BodyMemberType member, Collider collider)
    {
        switch (currentState)
        {
            case InteractableState.State1:
                PerformFirstStateAction(collider);
                break;
            case InteractableState.State2:
                PerformSecondStateAction(collider);
                break;
        }
    }

    protected abstract void PerformFirstStateAction(Collider collider);
    protected abstract void PerformSecondStateAction(Collider collider);


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
