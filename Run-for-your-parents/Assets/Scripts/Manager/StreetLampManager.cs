using UnityEngine;

public class StreetLampManager : MonoBehaviour, IGenerator
{
    #region Variables
    public enum LightType { unfonctional, disfonctional, fonctional, detector }

    [Tooltip("Type of the light")]
    [SerializeField]
    private LightType type = LightType.unfonctional;
    private Animator animator;
    private CapsuleCollider fieldDetector;

    private bool bugging = false;
    private float timer = 0f;
    private int bugs = 0;
    private bool on = true;


    #endregion

    #region Accessors

    private LightType Type
    {
        get => type;
        set
        {
            if (value == type) { return; }
            type = value;
            LightTypeUpdated();
        }
    }


    #endregion


    #region Built-in
    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
        fieldDetector = GetComponent<CapsuleCollider>();

        LightTypeUpdated();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (type == LightType.unfonctional || type == LightType.fonctional) { return; }

        if (bugging)
        {
            timer += Time.deltaTime;
            if (timer > Random.Range(0.1f, 2)) { ChangeLight(); }
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > 4)
            {
                bugging = true;
                timer = 0;
            }
        }

    }

    public void Generate()
    {
        Type = (LightType)Random.Range(0, System.Enum.GetValues(typeof(LightType)).Length);
        LightTypeUpdated();
    }

    #endregion

    #region Methods
    private void LightTypeUpdated()
    {
        if (!Application.isPlaying) { return; }

        animator.SetTrigger(type == LightType.unfonctional ? "off" : "on");
        fieldDetector.enabled = type == LightType.detector;
    }


    private void ChangeLight()
    {
        timer = 0;
        if (on)
        {
            animator.SetTrigger(type == LightType.disfonctional ? "off" : "detector");
            on = !on;
        }
        else
        {
            animator.SetTrigger("on");
            on = !on;

            if (++bugs >= 6)
            {
                bugging = false;
                bugs = 0;
            }
        }
    }


    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}