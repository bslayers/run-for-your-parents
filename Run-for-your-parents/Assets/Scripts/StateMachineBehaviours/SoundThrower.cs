using UnityEngine;

public class SoundThrower : StateMachineBehaviour
{
    #region Variables

    public enum WhenType { Enter, Exit, AtTime }

    [SerializeField]
    private WhenType whenThrow;

    [SerializeField]
    private float atTime;

    [SerializeField]
    private SoundData sound;

    private bool soundNotThrowed = false;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        soundNotThrowed = whenThrow == WhenType.AtTime;

        if (whenThrow == WhenType.Enter) { ThrowSound(animator); }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (soundNotThrowed && stateInfo.normalizedTime >= atTime) { ThrowSound(animator); soundNotThrowed = false; }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (whenThrow == WhenType.Exit) { ThrowSound(animator); }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    #endregion

    #region Methods

    private void ThrowSound(Animator animator)
    {
        if (sound == null) { return; }
        SoundEmitor.EmiteSound(animator.gameObject, sound);
    }


    #endregion

    #region Coroutine


    #endregion

}