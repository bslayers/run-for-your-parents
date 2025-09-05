using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class ChaseState : EnemyStateSO
{
    private float loseTargetTimer = 0f;
    public float maxLoseTargetTime = 5f;

    private float alertTime = 0f;
    private float alertInterval = 1.5f;

    private AudioSource chaserAudio;
    private Animator animator;
    private HashSet<int> alertedEnemies = new HashSet<int>();
    private float alertRadius;

    private NavMeshAgent agent;

    public override void Enter()
    {
        loseTargetTimer = 0f;
        alertTime = 0f;
        alertedEnemies.Clear();
        agent = enemy.Agent;

        if (enemy is Chaser chaser)
        {
            alertRadius = chaser.alertRadius;
            enemy.Agent.speed = chaser.runSpeed;
        }

        chaserAudio = enemy.GetComponent<AudioSource>();
        animator = enemy.animator;

        chaserAudio.Play();
        if (animator != null) { animator.SetBool("chasing", true); }
    }

    /// <summary>
    /// Called when the state is exited. Stops any ongoing audio and disables the chase animation.
    /// </summary>
    public override void Exit()
    {
        chaserAudio.Stop();

        if (animator != null) { animator.SetBool("chasing", false); }

        if (enemy is Chaser chaser) { agent.speed = chaser.walkSpeed; }

    }

    /// <summary>
    /// Performs the chasing behavior of the enemy. Updates the target destination and checks if the target is lost.
    /// </summary>
    public override void Perform()
    {
        if (chaserAudio.isPlaying && Time.deltaTime == 0) { chaserAudio.Pause(); }
        else if (!chaserAudio.isPlaying && Time.deltaTime == 1) { chaserAudio.UnPause(); }


        if (enemy.Target == null) { stateMachine.ChangeState(new PatrolState()); return; }

        if (enemy.CanSeeTarget() != Enemy.SeeState.OutOfSightField)
        {

            loseTargetTimer = 0f;
            if (IsNearToTarget()) { agent.SetDestination(enemy.transform.position); }
            else { agent.SetDestination(enemy.Target.transform.position); }

            enemy.transform.LookAt(enemy.Target.transform);

            alertTime += Time.deltaTime;

            if (alertTime >= alertInterval)
            {
                AlertNearbyEnemies();
                alertTime = 0f;
            }
        }
        else
        {
            chaserAudio.Pause();

            loseTargetTimer += Time.deltaTime;
            agent.SetDestination(enemy.LastTargetLocation);

            if (loseTargetTimer >= maxLoseTargetTime)
            {
                enemy.Target = null;
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    /// <summary>
    /// Alerts nearby enemies within a certain radius of the chase.
    /// </summary>
    private void AlertNearbyEnemies()
    {
        Collider[] allColliders = Physics.OverlapSphere(enemy.transform.position, alertRadius);

        foreach (var collider in allColliders)
        {
            if (collider.gameObject == enemy.gameObject) continue;

            Enemy otherEnemy = collider.GetComponentInParent<Enemy>();
            if (otherEnemy == null || otherEnemy == enemy) continue;

            if (otherEnemy.GetComponent<Chaser>() != null) continue;

            UpdateEnemy(otherEnemy);

        }
    }

    private bool IsNearToTarget()
    {
        return Vector3.Distance(enemy.transform.position, enemy.Target.transform.position) < 3f;
    }

    /// <summary>
    /// Updates the target for another enemy that was alerted.
    /// </summary>
    /// <param name="enemyDetector">The enemy to update the target for.</param>
    private void UpdateEnemy(Enemy enemyDetector)
    {
        if (enemyDetector == null || enemy.Target == null) return;
        enemyDetector.Target = enemy.Target;
    }

}
