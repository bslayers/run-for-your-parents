using UnityEngine;
using UnityEngine.AI;

public abstract class EnnemyBase : ActorSO
{
#region Variables
    protected StateMachine stateMachine;

    [Tooltip("The state machine that will be used to manage the ennemy's states.")]
    [SerializeField] protected EnemyStateSO startingState;

    private NavMeshAgent agent;
    protected GameObject target;

    [Header("Sight Values")]
    [Tooltip("The sight detection distance of the ennemy.")]
    public float sightDistance = 20f;
    [Tooltip("The field of view angle of the ennemy.")]
    public float fieldOfView = 85f;
    [Tooltip("The height of the ennemy's eyes for sight detection.")]
    public float eyesHeight;

    [Header("For Gizmos")]
    [Tooltip("Draw the field of view in the editor.")]
    [SerializeField] protected bool alwaysDrawFieldOfView;
    [Tooltip("The color of the field of view gizmos.")]
    public Color debugColour = Color.cyan;

#endregion

#region Accessors


public NavMeshAgent Agent{get => agent;}

#endregion

#region Built-in
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#endregion

#region Methods

    /// <summary>
    /// Check if target is in the sight of ennemy.
    /// </summary>
    /// <returns></returns>
    public bool CanSeeTarget()
    {
        if(!target) return false;
        
        if(Vector3.Distance(transform.position, target.transform.position) >= sightDistance) {return false;}

        Vector3 targetDirection = target.transform.position - transform.position - (Vector3.up * eyesHeight);
        float angleToTarget = Vector3.Angle(transform.forward, targetDirection);

        if(!(angleToTarget >= -(fieldOfView/2) && angleToTarget <= (fieldOfView/2))) {return false;}
        
        Ray ray = new Ray(transform.position + (Vector3.up * eyesHeight), targetDirection);
            
        RaycastHit hitInfo = new RaycastHit();
        if(!Physics.Raycast(ray, out hitInfo, sightDistance)) {return false;}

        //for debug drawing
        Debug.DrawLine(ray.origin, ray.direction * sightDistance);

        return hitInfo.transform.gameObject == target;
    }

#endregion


#region Events



#endregion

#region Editor
#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if(alwaysDrawFieldOfView)
        {
            DrawFieldOfView();
        }
    }

    private void DrawFieldOfView()
    {
        Gizmos.color = debugColour;
        DrawField(transform, sightDistance, 10,  fieldOfView);
    }

    void DrawField(Transform center, float radius, int segments, float sweepAngle)
    {
        float startAngle = -sweepAngle / 2f;
        float endAngle = sweepAngle / 2f;

        Vector3 startDirection = Quaternion.Euler(0, startAngle, 0) * center.forward;
        Vector3 endDirection = Quaternion.Euler(0, endAngle, 0) * center.forward;

        Vector3 previousPoint = center.position + startDirection * radius;

        if(sweepAngle != 360)
        {
            Gizmos.DrawLine(center.position, previousPoint);
            Gizmos.DrawLine(center.position, center.position + endDirection * radius);
        }


        // Dessiner un arc en segments
        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Lerp(startAngle, endAngle, i / (float)segments);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * center.forward;
            Vector3 newPoint = center.position + direction * radius;
            Gizmos.DrawLine(previousPoint, newPoint);
            previousPoint = newPoint;
        }
    }
#endif
#endregion
}
