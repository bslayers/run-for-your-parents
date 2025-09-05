using UnityEngine;

public class PutItemBehaviour : PlayerAnimatorStateSO
{
    [SerializeField]
    private Hand hand;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animatorManager.handManagers[hand]?.ShowCurrentItem();

        animatorManager.isShowingItem = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        animatorManager.handManagers[hand]?.HideAllItem();
        animatorManager.isShowingItem = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    base.OnStateMove(animator, stateInfo, layerIndex);
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //      base.OnStateIK(animator, stateInfo, layerIndex);
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

}
