using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private Player player;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    public IndexedHandManagerList handManagers;


    private static string rightHandAnimationName = "Right";
    private static string leftHandAnimationName = "Left";

    [Header("Animation name part")]

    [SerializeField]
    private string showingItemAnimationName = "ShowingItem";
    [SerializeField]
    private string losingItemAnimationName = "LoseItem";

    private IndexedBoolHandList objectRotationInHandFollowCamera = new() { list = { false, false } };

    [DisableField]
    public bool isShowingItem = false;
    [DisableField]
    public bool isTripping = false;

    #endregion

    #region Accessors

    public IndexedBoolHandList ObjectRotationInHandFollowCamera { get => objectRotationInHandFollowCamera; }


    #endregion


    #region Built-in

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        handManagers = player.handManagers;
    }

    #endregion

    #region Methods

    public void SetSpeed(float value)
    {
        animator.SetFloat("Speed", value);
    }

    public void SetDirection(Vector3 direction)
    {
        animator.SetFloat("XDirection", direction.x);
        animator.SetFloat("ZDirection", direction.z);
    }

    public void SetMoving(bool value)
    {
        animator.SetBool("IsMoving", value);
    }

    public void SetRunning(bool value)
    {
        animator.SetBool("IsRunning", value);
    }

    public void SetCrawling(bool value)
    {
        animator.SetBool("IsCrawling", value);
        if (value) { animator.SetTrigger("Fall"); }
    }

    public void SetSlithering(bool value)
    {
        animator.SetBool("IsSlithering", value);
    }

    public void SetSneeking(bool value)
    {
        animator.SetBool("IsSneeking", value);
    }

    public void SetGrounding(bool value)
    {
        animator.SetBool("IsGrounding", value);
    }

    public void SetJumping(bool value)
    {
        animator.SetBool("IsJumping", value);
    }

    public void TriggerLoseItem(Hand hand)
    {
        switch (hand)
        {
            case Hand.Left:
                animator.SetTrigger(leftHandAnimationName + losingItemAnimationName);
                break;
            case Hand.Right:
                animator.SetTrigger(rightHandAnimationName + losingItemAnimationName);
                break;
        }
    }

    public void SetShowItem(Hand hand, bool value)
    {
        switch (hand)
        {
            case Hand.Left:
                animator.SetBool(leftHandAnimationName + showingItemAnimationName, value);
                break;
            case Hand.Right:
                animator.SetBool(rightHandAnimationName + showingItemAnimationName, value);
                break;
        }
    }


    #endregion

    #region Coroutine


    #endregion

    #region Events


    #endregion

    #region Editor


    #endregion

}