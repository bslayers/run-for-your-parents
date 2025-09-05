using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ChangeChunkTracker))]
public class Enemy : MonoBehaviour
{
    #region Variables
    public enum TrackType { BaseTrack, SoundTrack, OrganismTrack, ChaseTrack };
    public enum SeeState { OutOfSightField, CantDigitalizeTarget, InDigitalizeField, NeedRotation, InSightDistance }

    protected StateMachine stateMachine;
    private ChangeChunkTracker changeChunkTracker;


    protected NavMeshAgent agent;
    protected GameObject target = null;
    private Vector3 lastTargetLocation = Vector3.zero;

    [Tooltip("The animator of the ennemy")]
    public Animator animator;
    [Tooltip("The type of track the ennemy will follow")]
    public TrackType chaseType = TrackType.BaseTrack;

    [Tooltip("The distance the ennemy can steal")]
    public float stealDistance = 12.5f;
    [Tooltip("The maximum distance the ennemy can see")]
    public float maxDistanceAroundTarget = 10f;
    public float timeToSteal = 2.5f;

    [Header("Sight Values")]
    [Tooltip("The distance the ennemy can see")]
    public float sightDistance = 20f;
    [Tooltip("The field of view of the ennemy")]
    public float fieldOfView = 170f;
    [Tooltip("The height of the ennemy's eyes")]
    public float eyesHeight;

    public float patrolRadius;

    [Header("Audio sources")]
    [SerializeField]
    private AudioSource existingSound;
    [SerializeField]
    private AudioSource digitalizeSound;

    [Header("For Gizmos")]
    [Tooltip("If true, the ennemy will always draw the field of view")]
    [SerializeField] protected bool alwaysDrawFieldOfView;
    [Tooltip("The color of the field of view")]
    public Color debugColour = Color.cyan;

    //For memorising the the power of the last sound heard
    private int currentSoundStrength = -1;
    //It's to determine how long the enemy is still listening to this sound.
    private float soundTimer = 0f;


    #endregion

    #region Accessors

    public NavMeshAgent Agent { get => agent; }

    public GameObject Target
    {
        get
        {
            return target;
        }
        set
        {
            if (target == value && value != null)
            {
                lastTargetLocation = target.transform.position;
                return;
            }

            target = value;

            if (target != null)
            {
                lastTargetLocation = target.transform.position;
                NewTargetBehaviour();
            }
            else
            {
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    public Vector3 LastTargetLocation
    {
        get
        {
            return lastTargetLocation;
        }
    }


    #endregion


    #region Built-in
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        changeChunkTracker = GetComponent<ChangeChunkTracker>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateIsMoving();
        UpdateSound();

        if (soundTimer > 0f)
        {
            soundTimer -= Time.deltaTime;
            if (soundTimer <= 0f)
            {
                currentSoundStrength = -1;
                Target = null;
            }
        }

    }

    protected virtual void StartBehaviour()
    {
        stateMachine.Initialize();
    }


    #endregion

    #region Methods

    protected void UpdateSound()
    {
        bool timeIsRunning = Time.deltaTime > 0;
        if (existingSound != null && existingSound.clip != null) 
        {
            if (existingSound.isPlaying && !timeIsRunning) { existingSound.Pause(); }
            else if (!existingSound.isPlaying && timeIsRunning) { existingSound.UnPause(); }
        }

        if(digitalizeSound != null && digitalizeSound.clip != null)
        {
            if (digitalizeSound.isPlaying && !timeIsRunning) { digitalizeSound.Pause(); }
            else if (!digitalizeSound.isPlaying && timeIsRunning) { digitalizeSound.UnPause(); }
        }


    }

    protected virtual void NewTargetBehaviour()
    {
        switch (chaseType)
        {
            case TrackType.SoundTrack:
                if (stateMachine?.currentState?.GetType() != typeof(TrackSoundState)) { stateMachine.ChangeState(new TrackSoundState()); }
                break;
            case TrackType.OrganismTrack:
                if (stateMachine?.currentState?.GetType() != typeof(TrackOrganismState)) { stateMachine.ChangeState(new TrackOrganismState()); }
                break;
            case TrackType.ChaseTrack:
                if (stateMachine?.currentState?.GetType() != typeof(TrackChaserState)) { stateMachine.ChangeState(new TrackChaserState()); }
                break;

        }
    }

    private void UpdateIsMoving()
    {
        if (!agent) { return; }
        float currentSpeed = agent.velocity.magnitude;
        changeChunkTracker.IsMoving = currentSpeed >= 0.5;
    }

    //TODO : update Doc
    /// <summary>
    /// Check if target is in the sight of ennemy and if ennemy can digitalize it.
    /// </summary>
    /// <returns></returns>
    public SeeState CanSeeTarget()
    {
        if (!target) return SeeState.OutOfSightField;

        Vector3 targetPosition = target.transform.position;
        Vector3 targetDirection = target.transform.position - transform.position - (Vector3.up * eyesHeight);
        float angleToTarget = Vector3.Angle(transform.forward, targetDirection);
        Ray ray = new(transform.position + (Vector3.up * eyesHeight), targetDirection);

        bool inSightDistance = Vector3.Distance(transform.position, target.transform.position) >= sightDistance;

        if (inSightDistance) { return SeeState.OutOfSightField; }

        bool inSightField = (angleToTarget >= -(fieldOfView / 2) && angleToTarget <= (fieldOfView / 2));
        bool InStealDistance = Physics.Raycast(ray, out RaycastHit hitInfo, stealDistance);

        //if(!inSightField && canDigitalize) { return SeeState.NeedRotation; }

        if (!inSightField) { return SeeState.OutOfSightField; }

        if (!InStealDistance) { return SeeState.CantDigitalizeTarget; }


        #if UNITY_EDITOR
        //for debug drawing
        Debug.DrawLine(ray.origin, ray.direction * stealDistance);
        #endif
        
        GameObject origin = hitInfo.transform.gameObject;
        if (hitInfo.transform.CompareTag("Player"))
        {
            GetPlayerOrigineByHitInfo(hitInfo, ref origin);
        }

        return origin == target ? SeeState.InDigitalizeField : SeeState.CantDigitalizeTarget;
    }

    private void GetPlayerOrigineByHitInfo(RaycastHit hitInfo, ref GameObject res)
    {
        MemberCollider collider = hitInfo.transform.gameObject.GetComponent<MemberCollider>();
        if (collider == null) { return; }
        res = collider.Origin;
    }

    public void ChangeTargetIfSoundHigher(GameObject newTarget, int strength, float duration)
    {
        if (newTarget == target) { lastTargetLocation = target.transform.position; }
        if (target == null || soundTimer <= 0 || strength > currentSoundStrength)
        {
            Target = newTarget;
            currentSoundStrength = strength;
            soundTimer = duration;
        }
    }

    public void DigitalizeTargetFeedback()
    {
        if (digitalizeSound == null || digitalizeSound.isPlaying) { return; }
        digitalizeSound.Play();
    }

    public void NoSeeTargetFeedback()
    {
        if (digitalizeSound == null || !digitalizeSound.isPlaying) { return; }
        digitalizeSound.Stop();
    }

    #endregion


    #region Events

    void OnEnable()
    {
        StartCoroutine(DelayedStartBehaviour());
    }


    #endregion

    #region Coroutines

    IEnumerator DelayedStartBehaviour()
    {
        yield return new WaitForSeconds(1f);
        if (agent.isOnNavMesh)
        {
            StartBehaviour();
        }
    }

    #endregion

    #region Editor
#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (alwaysDrawFieldOfView)
        {
            DrawFieldOfView();
            DrawLineToTarget();
        }
    }

    private void DrawFieldOfView()
    {
        Gizmos.color = debugColour;
        UtilGizmo.DrawField(transform, sightDistance, 10, fieldOfView);
        Gizmos.color = Color.red;
        UtilGizmo.DrawField(transform, stealDistance, 10, fieldOfView);

    }

    private void DrawLineToTarget()
    {
        if (target == null) { return; }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, lastTargetLocation);
    }

#endif
    #endregion


}
