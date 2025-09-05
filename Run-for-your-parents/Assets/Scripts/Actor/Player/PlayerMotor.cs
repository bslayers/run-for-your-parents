using UnityEngine;

[RequireComponent(typeof(PlayerInputsManager))]
public class PlayerMotor : PlayerComponentSO
{
    #region Variables
    private PlayerAnimatorManager animatorManager;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private PlayerData data;


    [Header("Informations for motor")]
    [Tooltip("The player is grounded or not")]
    public bool isGrounded;
    private float gravity;
    private float jumpHight;

    private float maxSpeed;

    private bool isRunning = false;
    private bool isSneeking = false;
    private bool isCrawling = false;
    private bool isSlithering = false;

    private float verticalVelocity = 0f;

    private Vector3 lastPosition;
    private Vector3 LastDirection;


    #endregion

    #region Accessors

    public bool IsRunning
    {
        get => isRunning;
        private set
        {
            if (value == isRunning) { return; }
            isRunning = value;
            animatorManager.SetRunning(isRunning);
        }
    }

    public bool IsSneeking
    {
        get => isRunning;
        private set
        {
            if (isSneeking == value) { return; }
            isSneeking = value;
            animatorManager.SetSneeking(isSneeking);
        }
    }

    public bool IsCrawling
    {
        get => isRunning;
        private set
        {
            if (value == isCrawling) { return; }
            isCrawling = value;
            animatorManager.SetCrawling(isCrawling);
        }
    }

    public bool IsSlithering
    {
        get => isRunning;
        private set
        {
            if (value == isSlithering) { return; }
            isSlithering = value;
            animatorManager.SetSlithering(isSlithering);
        }
    }

    #endregion


    #region Built-in

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        animatorManager.SetGrounding(isGrounded);

        animatorManager.SetSpeed(((transform.position - lastPosition) / Time.deltaTime).magnitude);
        if (lastPosition.y < transform.position.y) { animatorManager.SetJumping(false); }
        lastPosition = transform.position;

    }

    protected override void InitPlayer()
    {
        base.InitPlayer();
        controller = GetComponent<CharacterController>();
        animatorManager = player.animatorManager;
        data = player.playerData;
    }

    protected override void StartPlayer()
    {
        base.StartPlayer();
        maxSpeed = data.speeds.walkingSpeed;
        lastPosition = transform.position;
        gravity = data.gravity;
        jumpHight = data.jumpHight;
    }

    #endregion

    #region Methods
    /// <summary>
    /// Recieve the inputs for our InputsManager.cs
    /// </summary>
    /// <param name="input"></param>
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        if (animatorManager.isTripping)
        {
            verticalVelocity += gravity * Time.deltaTime;
            moveDirection.y = verticalVelocity;

            moveDirection += transform.forward * 0.5f;
        }
        else if (isGrounded)
        {
            verticalVelocity = -2f;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            LastDirection = moveDirection;
        }
        else
        {
            moveDirection = LastDirection;
        }

        animatorManager.SetMoving(moveDirection != Vector3.zero);
        animatorManager.SetDirection(moveDirection);

        controller.Move(transform.TransformDirection(moveDirection) * maxSpeed * Time.deltaTime);

        if (isGrounded && playerVelocity.y < 0) playerVelocity.y = -2f;
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    /// <summary>
    /// Jump behaviour
    /// </summary>
    public void Jump()
    {
        if (!isGrounded || isCrawling || isSlithering) { return; }
        playerVelocity.y = Mathf.Sqrt(jumpHight * -3.0f * gravity);
        animatorManager.SetJumping(true);
    }

    /// <summary>
    /// Crouch behaviour
    /// </summary>
    public void Crouch()
    {
        if (isCrawling || isSlithering) { return; }

        IsSneeking = !isSneeking;
        maxSpeed = isSneeking ? data.speeds.sneekingSpeed : data.speeds.walkingSpeed;
    }

    /// <summary>
    /// Sprint behaviour
    /// </summary>
    public void Sprint()
    {
        if (isCrawling || isSlithering) { return; }

        IsRunning = !isRunning;
        maxSpeed = isRunning ? data.speeds.runningSpeed : data.speeds.walkingSpeed;
    }

    /// <summary>
    /// Crawl behaviour
    /// </summary>
    public void Crawl()
    {
        if (isCrawling) { return; }

        IsCrawling = true;
        maxSpeed = data.speeds.crawlingSpeed;
    }

    public void Slither()
    {
        if (isSlithering) { return; }

        IsSlithering = true;

    }

    #endregion


    #region Events


    #endregion

    #region Editor


    #endregion
}
