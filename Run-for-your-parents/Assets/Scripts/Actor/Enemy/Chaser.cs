using UnityEngine;

public class Chaser : Enemy
{
    [Header("Chaser configuration")]
    [Tooltip("Time required to detect the player before starting the chase.")]
    public float timeToConfirmTarget = 2f;
    private float playerDetectionTimer = 0f;

    [Tooltip("The radius within which nearby enemies will be alerted when the player is detected.")]
    public float alertRadius = 30f;

    [Tooltip("The run speed of the monster when the enemy is running")]
    public float runSpeed = 5f;

    [Tooltip("The walk speed of the monster when the enemy is walking")]
    public float walkSpeed = 3.5f;

    protected override void StartBehaviour()
    {   
        stateMachine.ChangeState(new PatrolState());
    }

    protected override void Update()
    {
        base.Update();

        CheckForPlayerDetection();

        if (animator != null)animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    protected override void NewTargetBehaviour()
    {
        if(stateMachine.currentState is ChaseState) { return; }
        base.NewTargetBehaviour();
    }

    /// <summary>
    /// Checks if the player is within the detection radius. If so, starts the detection timer.
    /// If the player is still detected after the specified time, changes the state to ChaseState.
    /// </summary>
    private void CheckForPlayerDetection()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, sightDistance, LayerMask.GetMask("Player"));

        if (players.Length > 0)
        {
            GameObject player = players[0].gameObject;
            GameObject oldTarget = Target;
            target = player;

            if (CanSeeTarget() != SeeState.OutOfSightField)
            {
                playerDetectionTimer += Time.deltaTime;

                if (playerDetectionTimer >= timeToConfirmTarget)
                {
                    Target = player;
                    stateMachine.ChangeState(new ChaseState());
                    playerDetectionTimer = 0f;
                }
            }
            else
            {
                playerDetectionTimer = 0f;
                target = oldTarget;
            }
        }
    }
}
