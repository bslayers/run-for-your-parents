using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyStateSO
{
    #region Variables
    public float waitTimer;

    #endregion

    #region Accessors


    #endregion


    #region Built-in

    public override void Enter()
    {
        SetDestination();
    }

    public override void Perform()
    {
        PatrolCycle();
    }

    public override void Exit()
    {

    }

    #endregion

    #region Methods
    /// <summary>
    /// Patrol cycle behaviour
    /// </summary>
    private void PatrolCycle()
    {


        //implement patrol logic
        if (enemy.Agent.remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer < 3) return;

            SetDestination();

        }
    }

    private void SetDestination()
    {
        if (enemy.Agent == null || !enemy.Agent.isOnNavMesh) { return; }

        if (NavMesh.SamplePosition(GetNewPoint(), out NavMeshHit hit, enemy.patrolRadius, NavMesh.AllAreas))
        {
            enemy.Agent.SetDestination(hit.position);
        }
    }

    private Vector3 GetNewPoint()
    {
        Vector3 center = enemy.transform.position;
        Vector3 randomDirection = Random.insideUnitSphere * enemy.patrolRadius;
        return randomDirection + center;

    }


    #endregion


    #region Events


    #endregion


    #region Editor


    #endregion
}
