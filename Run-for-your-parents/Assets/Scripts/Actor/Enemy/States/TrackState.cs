using UnityEngine;
using UnityEngine.AI;

public abstract class TrackState : EnemyStateSO
{
    #region Variables
    private float moveTimer;
    private float stealTimer;
    private float losePlayerTimer;

    private float timeToSteal = 2.5f;

    private NavMeshAgent agent;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    public override void Enter()
    {
        agent = enemy.Agent;
        enemy.animator.SetBool("chasing", true);
        timeToSteal = enemy.timeToSteal;
    }

    public override void Exit()
    {
        enemy.animator.SetBool("chasing", false);
    }

    public override void Perform()
    {
        if (Time.deltaTime == 0) { return; }
        if (enemy.Target)
        {
            Enemy.SeeState status = enemy.CanSeeTarget();

            if (status == Enemy.SeeState.InDigitalizeField)
            {
                enemy.DigitalizeTargetFeedback();
                stealTimer += Time.deltaTime;
                agent.SetDestination(enemy.transform.position);
                enemy.transform.LookAt(enemy.Target.transform);
                if (stealTimer > timeToSteal) StealBehaviour();
            }
            else if (status == Enemy.SeeState.NeedRotation)
            {
                agent.SetDestination(enemy.transform.position);
                enemy.transform.LookAt(enemy.Target.transform);
            }
            else if (status == Enemy.SeeState.OutOfSightField && InAroundLastTargetLocation(enemy.maxDistanceAroundTarget))
            {
                //can't see target at his last known location: return to PatrolState
                enemy.Target = null;
            }
            else
            {
                enemy.NoSeeTargetFeedback();
                agent.SetDestination(enemy.LastTargetLocation);
                stealTimer = 0;
            }

        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                //change to the search state
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    /// <summary>
    /// Behaviour launched when the enenmy can steal something from the target 
    /// </summary>
    public abstract void StealBehaviour();

    #endregion

    #region Methods
    /// <summary>
    /// Check if ennemy is around the last target location, depending of <paramref name="maxDistance"/>
    /// </summary>
    /// <param name="maxDistance">radius of cercle to check</param>
    /// <returns>true if ennemy is around, false otherwise</returns>
    private bool InAroundLastTargetLocation(float maxDistance)
    {
        float distance = Vector3.Distance(enemy.transform.position, enemy.LastTargetLocation);
        //for debuging
        //Ray ray = new Ray(ennemy.transform.position, ennemy.LastTargetLocation - ennemy.transform.position);
        //Debug.DrawRay(ray.origin, ray.direction * maxDistance);
        return distance < maxDistance;
    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion

}