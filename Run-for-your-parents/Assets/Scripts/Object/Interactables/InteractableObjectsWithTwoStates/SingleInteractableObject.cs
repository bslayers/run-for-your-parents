using UnityEngine;

public class SingleInteractableObject : InteractableObjectWithTwoStatesSO
{
    #region Variables
    [SerializeField]
    private Animator animator;

    [Header("Sounds")]
    [SerializeField]
    private SoundData firstStateSound;
    [SerializeField]
    private SoundData secondStateSound;

    #endregion

    #region Accessors


    #endregion


    #region Built-in


    #endregion

    #region Methods
    protected override void PerformFirstStateAction(Collider collider)
    {
        animator.SetTrigger("PerformState1");
        currentState = InteractableState.State2;
        SoundEmitor.EmiteSound(gameObject, secondStateSound);
    }

    protected override void PerformSecondStateAction(Collider collider)
    {
        animator.SetTrigger("PerformState2");
        currentState = InteractableState.State1;
        SoundEmitor.EmiteSound(gameObject, firstStateSound);
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion

}