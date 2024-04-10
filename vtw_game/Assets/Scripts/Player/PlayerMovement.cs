using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    [Header("Player Stats")]
    public PlayerStats playerStats;

    [Header("Physics Components")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public Transform wallCheckEdge;
    public Transform wallCheck;
    public Transform wallCheckLower;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    [Header("Anchoring Settings")]
    private bool isAnchored = false;
    private float originalGravityScale;

    [Header("Climbing Settings")]
    private bool isClimbing = false;
    private float vertical;

    [Header("Swing Settings")]
    private float swingCooldown = 1f;
    private float lastJumpTime;
    public PlayerMovement otherPlayer;
    public bool isSwinging = false;

    [Header("Jump Settings")]
    private bool isJumping;

    [Header("Timing Settings")]
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float climbBufferCounter;

    [Header("Player State")]
    private float horizontal;
    private bool isGroundedThisFrame;
    private float lastHorizontalDirection = 0f;

    [Header("Audio SFX")]
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        originalGravityScale = rb.gravityScale;
    }


    void Update()
    {
        UpdateGroundedStatus();
        UpdateTimers();
        HandlePlayerDirection();
    }


    private void FixedUpdate()
    {
        HandleMovement();
    }


    #region GroundChecks
    void UpdateGroundedStatus()
    {
        isGroundedThisFrame = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (isGroundedThisFrame)
        {
            coyoteTimeCounter = playerStats.coyoteTime;
            isJumping = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }
    #endregion


    #region TimerUpdates
    void UpdateTimers()
    {
        if (jumpBufferCounter > 0) jumpBufferCounter -= Time.deltaTime;
        if (climbBufferCounter > 0) climbBufferCounter -= Time.deltaTime;
    }
    #endregion


    #region PlayerDirection
    void HandlePlayerDirection()
    {
        if ((horizontal > 0.01f && lastHorizontalDirection <= 0) || (horizontal < -0.01f && lastHorizontalDirection >= 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontal) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            lastHorizontalDirection = horizontal;
        }
    }
    #endregion


    #region MovementHandling
    void HandleMovement()
    {
        if (playerStats.canMove && !isAnchored)
        {
            HandleClimbing();
            if (!isClimbing)
            {
                GroundMovement();
                StartSwing();
            }
        }
        else if (isAnchored)
        {
            Anchor();
        }
    }
    #endregion


    #region HorizontalMovement Method
    void HorizontalMovement()
    {
        rb.velocity = new Vector2(horizontal * playerStats.speed, rb.velocity.y);
    }
    #endregion


    #region GroundMovement Methods
    void GroundMovement()
    {
        if (isGroundedThisFrame || Time.time - lastJumpTime < swingCooldown)
        {
            HorizontalMovement();
        }
    }
    #endregion


    #region Climbing Methods
    private void HandleClimbing()
    {
        bool nextToWall = IsNextToWall();
        bool isWallBelow = IsWallBelow();

        if (!isClimbing && climbBufferCounter > 0 && nextToWall)
        {
            StartClimbing();
        }

        if (isClimbing)
        {
            ClimbingPhysics();
            float climbingSpeed = CalculateClimbingSpeed(isWallBelow);
            rb.velocity = new Vector2(0, vertical * climbingSpeed);
            CheckForEdge();

            if (!nextToWall)
            {
                StopClimbing();
            }
        }
    }

    bool IsNextToWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer);

    }
    bool IsWallBelow()
    {
        return Physics2D.OverlapCircle(wallCheckLower.position, 0.1f, wallLayer);
    }
    private void ClimbingPhysics()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        rb.gravityScale = 0;
    }

    private float CalculateClimbingSpeed(bool isWallBelow)
    {
        if (vertical < 0 && !isWallBelow)
        {
            return 0;
        }
        else if (vertical > 0)
        {
            return playerStats.climbingSpeedUp;
        }
        else if (vertical < 0 && isWallBelow)
        {
            return playerStats.climbingSpeedDown;
        }
        return 0;
    }
    void StartClimbing()
    {
        isClimbing = true;
        audioManager.PlaySFX(audioManager.wallGrab);
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = originalGravityScale;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void CheckForEdge()
    {
        bool isEdgeDetected = !Physics2D.OverlapCircle(wallCheckEdge.position, 0.1f, wallLayer);
        if (isEdgeDetected)
        {
            StartCoroutine(PerformEdgeClimb());
        }
    }
    IEnumerator PerformEdgeClimb()
    {
        yield return new WaitForSeconds(playerStats.initialDelay);
        StopClimbing();
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        rb.AddForce(new Vector2(0, playerStats.verticalClimbForce), ForceMode2D.Impulse);
        yield return new WaitForSeconds(playerStats.climbPreparationDelay);
        rb.AddForce(new Vector2(direction * playerStats.horizontalClimbForce, 0), ForceMode2D.Impulse);
                audioManager.PlaySFX(audioManager.edgeClimb);

    }

    void ApplyHorizontalClimbingForce(float horizontalInput)
    {
        Vector2 horizontalForce = new Vector2(horizontalInput * playerStats.climbingHorizontalForce, 0);
        rb.AddForce(horizontalForce, ForceMode2D.Impulse);
        StartCoroutine(ApplyVerticalBoostAfterDelay());
    }

    IEnumerator ApplyVerticalBoostAfterDelay()
    {
        yield return new WaitForSeconds(playerStats.boostDelay);
        Vector2 verticalForce = new Vector2(0, playerStats.climbingVerticalBoost);
        rb.AddForce(verticalForce, ForceMode2D.Impulse);
    }
    #endregion


    #region Jump Methods
    void PerformJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, playerStats.jumpingPower);
        audioManager.PlaySFX(audioManager.jump);
        jumpBufferCounter = 0;
        isJumping = true;
    }
    #endregion


    #region Swing Methods
private bool hasPlayedSwingSound = false;

void StartSwing()
{
    bool otherPlayerSwinging = otherPlayer != null && otherPlayer.isSwinging;
    if (!otherPlayerSwinging && !(isGroundedThisFrame || Time.time - lastJumpTime < swingCooldown) && Mathf.Abs(horizontal) > 0.01f)
    {
        isSwinging = true;
        
        // Check if the swing sound has not been played yet
        if (!hasPlayedSwingSound)
        {
            audioManager.PlaySFX(audioManager.startSwing);
            hasPlayedSwingSound = true; // Set the flag to true to indicate that the sound has been played
        }
        
        AirControlImpulse(horizontal);
    }
    else
    {
        isSwinging = false;
        if (Time.time - lastJumpTime < swingCooldown)
        {
            HorizontalMovement();
        }
        
        // Reset the flag when the player stops swinging
        hasPlayedSwingSound = false;
    }
}

    void AirControlImpulse(float direction)
    {
        float currentHorizontalVelocity = rb.velocity.x;
        if ((direction > 0 && currentHorizontalVelocity > 0) || (direction < 0 && currentHorizontalVelocity < 0))
        {
            rb.AddForce(new Vector2(playerStats.airControl * direction, 0), ForceMode2D.Impulse);
        }
    }
    #endregion


    #region Anchor Methods
    void Anchor()
    {
        rb.velocity = Vector2.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    #endregion


    #region Input Actions
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        horizontal = moveInput.x;
        vertical = moveInput.y;

        if (isGroundedThisFrame && moveInput.y < 0 && !isAnchored)
        {
            isAnchored = true;
        }
        else if (isAnchored && (moveInput.y >= 0 || !isGroundedThisFrame))
        {
            isAnchored = false;
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        if (isClimbing && horizontal != 0)
        {
            ApplyHorizontalClimbingForce(horizontal);
        }
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (!playerStats.canJump || isAnchored || isClimbing) return;
        if (context.performed)
        {
            lastJumpTime = Time.time;
            jumpBufferCounter = playerStats.jumpBufferTime;
        }

        if ((isGroundedThisFrame || playerStats.canDoubleJump || coyoteTimeCounter > 0) && jumpBufferCounter > 0 && !isJumping)
        {
            PerformJump();
            coyoteTimeCounter = 0;
        }
        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    public void Climb(InputAction.CallbackContext context)
    {
        if (context.started && playerStats.canClimb && IsNextToWall())
        {
            climbBufferCounter = playerStats.climbBufferTime;
        }
        else if (context.canceled && isClimbing)
        {
            StopClimbing();
        }
    }
    #endregion
}




