using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Stats")]
    public PlayerStats playerStats;

    [Header("Physics Components")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public Transform wallCheckUpper;
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
    private float swingCooldown = 0.5f;
    private float lastJumpTime;

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

    #region Ground Checks
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

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    #endregion

    #region Timer Updates
    void UpdateTimers()
    {
        if (jumpBufferCounter > 0) jumpBufferCounter -= Time.deltaTime;
        if (climbBufferCounter > 0) climbBufferCounter -= Time.deltaTime;
    }
    #endregion

    #region Player Direction
    void HandlePlayerDirection()
    {
        if ((horizontal > 0.01f && lastHorizontalDirection <= 0) || (horizontal < -0.01f && lastHorizontalDirection >= 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontal) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            lastHorizontalDirection = horizontal;
        }
    }
    #endregion

    #region Movement Handling
    void HandleMovement()
    {
        if (playerStats.canMove && !isAnchored)
        {
            bool nextToWall = IsNextToWall();
            bool isWallAbove = Physics2D.OverlapCircle(wallCheckUpper.position, 0.1f, wallLayer);
            bool isWallBelow = Physics2D.OverlapCircle(wallCheckLower.position, 0.1f, wallLayer);

            if (!isClimbing && climbBufferCounter > 0 && nextToWall)
            {
                StartClimbing();
            }

            if (isClimbing && nextToWall)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                rb.gravityScale = 0;

                float currentClimbingSpeed = 0f;
                if (vertical > 0 && isWallAbove)
                {
                    currentClimbingSpeed = playerStats.climbingSpeedUp;
                }
                else if (vertical < 0 && isWallBelow)
                {
                    currentClimbingSpeed = playerStats.climbingSpeedDown;
                }

                rb.velocity = new Vector2(0, vertical * currentClimbingSpeed);
            }
            else if (!nextToWall)
            {
                StopClimbing();
            }

            if (!isClimbing)
            {
                if (isGroundedThisFrame || Time.time - lastJumpTime < swingCooldown)
                {
                    rb.velocity = new Vector2(horizontal * playerStats.speed, rb.velocity.y);
                }
                else if (Mathf.Abs(horizontal) > 0.01f)
                {
                    OptimizeAirControlImpulse(horizontal);
                }
            }
        }
        else if (isAnchored)
        {
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    bool IsNextToWall()
    {
        return Physics2D.OverlapCircle(wallCheckUpper.position, 0.1f, wallLayer) || Physics2D.OverlapCircle(wallCheckLower.position, 0.1f, wallLayer);
    }

    void OptimizeAirControlImpulse(float direction)
    {
        float currentHorizontalVelocity = rb.velocity.x;
        if ((direction > 0 && currentHorizontalVelocity > 0) || (direction < 0 && currentHorizontalVelocity < 0))
        {
            rb.AddForce(new Vector2(playerStats.airControl * direction, 0), ForceMode2D.Impulse);
        }
    }
    #endregion

    #region Climbing Methods
    void StartClimbing()
    {
        isClimbing = true;
        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = originalGravityScale;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
    }
    #endregion

    #region Jump Methods
    void PerformJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, playerStats.jumpingPower);
        jumpBufferCounter = 0;
        isJumping = true;
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
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!playerStats.canJump || isAnchored || isClimbing) return;

        if (context.performed)
        {
            lastJumpTime = Time.time;
            jumpBufferCounter = playerStats.jumpBufferTime;
        }

        if ((isGroundedThisFrame || playerStats.canDoubleJump) && jumpBufferCounter > 0 && !isJumping)
        {
            PerformJump();
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
