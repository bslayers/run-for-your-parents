using UnityEngine;

[AddComponentMenu("Actor")]
public abstract class ActorSO : MonoBehaviour
{
    #region Variables
    public Animator animator;

    [Header("Actor configuration")]
    [Tooltip("The maximum health of the actor")]
    [SerializeField] protected int maxHealth = 10;
    private int health;
    [Tooltip("The actor will start with the maximum health")]
    [SerializeField] protected bool startWithMaxHealth = true;
    [Tooltip("The actor will start with the starting health")]
    [SerializeField] protected int startingHealth = 10;
    #endregion

    #region Accessors

    public int Health
    {
        get
        {
            return health;
        }
    }
    #endregion

    #region Built-in

    // Start is called once before the execution of Start after the MonoBehaviour is created
    void Awake()
    {
        ActorInit();
    }

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

    protected virtual void ActorInit()
    {
        InitHealth();

        /*
        ProceduralGenerationManager proceduralGenerationManager = FindAnyObjectByType<ProceduralGenerationManager>();
        if (proceduralGenerationManager != null)
        {
            Events.Instance.GenerationFinished.AddListener(OnEventsGenerationFinished);
            gameObject.SetActive(false);
        }
        */
    }

    protected virtual void InitHealth()
    {
        if (startWithMaxHealth)
        {
            health = startingHealth;
        }
        else
        {
            health = maxHealth;
        }
    }

    public virtual void Hurt(int dammage)
    {
        health -= dammage;
        HurtFeedback();

        if (health <= 0)
        {
            Death();
        }
    }
    protected abstract void HurtFeedback();

    protected virtual void Death()
    {
        DeathFeedback();
    }
    protected abstract void DeathFeedback();

    public virtual void Heal(int heal)
    {
        health += heal;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        HealFeedback();
    }
    protected abstract void HealFeedback();

    #endregion

    #region Events

    protected void OnEventsGenerationFinished()
    {
        gameObject.SetActive(true);
        Events.Instance.GenerationFinished.RemoveListener(OnEventsGenerationFinished);
    }

    #endregion

    #region Editor



    #endregion
}
