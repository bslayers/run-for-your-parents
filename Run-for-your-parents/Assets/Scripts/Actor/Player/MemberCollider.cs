using UnityEngine;

public class MemberCollider : MonoBehaviour
{
    #region Variables

    protected float timeInDetector = 0f;

    [SerializeField]
    protected float timeBeforeWorseDigitalizing = 1.5f;

    [SerializeField, RequiredReference]
    protected Player player;

    protected PlayerBodyManager bodyManager;
    protected PlayerMotor motor;



    public BodyMemberType member;

    protected bool inDetector = false;

    protected int nbCollision = 0;

    #endregion

    #region Accessors

    public GameObject Origin { get => player.gameObject; }

    #endregion


    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        AwakeMemberCollider();
    }

    protected virtual void AwakeMemberCollider()
    {

        bodyManager = player.GetComponent<PlayerBodyManager>();
        motor = player.GetComponent<PlayerMotor>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartMemberCollider();
    }

    protected virtual void StartMemberCollider()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMemberCollider();
    }

    protected virtual void UpdateMemberCollider()
    {
        if (inDetector) { timeInDetector += Time.deltaTime; }
    }

    #endregion

    #region Methods

    private void EnterDetector()
    {
        inDetector = true;
        timeInDetector = 0;
    }

    private void ExitDetector()
    {
        inDetector = false;

        if (timeInDetector < timeBeforeWorseDigitalizing)
        {
            bodyManager.SetExisting(member, false);
        }
        else if (timeInDetector < timeBeforeWorseDigitalizing * 2)
        {
            switch (member)
            {
                case BodyMemberType.RightFoot:
                case BodyMemberType.RightLeg:
                    bodyManager.SetExisting(BodyMemberType.RightFoot, false);
                    bodyManager.SetExisting(BodyMemberType.RightLeg, false);
                    break;
                case BodyMemberType.LeftLeg:
                case BodyMemberType.LeftFoot:
                    bodyManager.SetExisting(BodyMemberType.LeftFoot, false);
                    bodyManager.SetExisting(BodyMemberType.LeftLeg, false);
                    break;
                case BodyMemberType.RightHand:
                case BodyMemberType.RightArm:
                    bodyManager.SetExisting(BodyMemberType.RightHand, false);
                    bodyManager.SetExisting(BodyMemberType.RightArm, false);
                    break;
                case BodyMemberType.LeftHand:
                case BodyMemberType.LeftArm:
                    bodyManager.SetExisting(BodyMemberType.LeftHand, false);
                    bodyManager.SetExisting(BodyMemberType.LeftArm, false);
                    break;
            }
        }
        else
        {
            bodyManager.Digitalize();
        }
    }

    protected virtual void MakeSound() { }


    #endregion

    #region Coroutine


    #endregion

    #region Events

    void OnCollisionEnter(Collision collision)
    {
        ++nbCollision;
        if (nbCollision == 1) { MakeSound(); }

        if (!collision.collider.CompareTag("Detector")) { return; }

        if (collision.collider.GetComponent<DetectorSO>()?.IsActive ?? false) { EnterDetector(); }
    }


    void OnCollisionExit(Collision collision)
    {
        --nbCollision;

        if (!collision.collider.CompareTag("Detector")) { return; }

        if (inDetector) { ExitDetector(); }
    }

    #endregion

    #region Editor


    #endregion

}