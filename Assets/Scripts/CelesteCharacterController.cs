using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelesteCharacterController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public float coyoteTime = 0.1f;
    public float jumpBufferDuration = 0.1f;
    public float dashForce = 20.0f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 0.5f;
    public float dashDirectionSmoothness = 5.0f;
    public float airControl = 1.0f;
    public float climbSpeed = 10.0f;
    public float wallSlideSpeed = 5.0f;
    public float stamina = 100.0f;
    public float staminaRechargeRate = 10.0f;
    public float staminaDrainRate = 5.0f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private BoxCollider2D collider;
    private Animator animator;
    private bool isDashing = false;
    private float dashTimer = 0.0f;
    private float dashCoolDownTimer = 0.0f;
    private bool hasDashedAfterJump = false;
    private bool isClimbing = false;
    private Vector2 wallJumpDirection;
    private bool isFacingRight = true;
    private float jumpBufferTimer = 0.0f;
    private bool canClimb = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        if(GetComponent<Animator>() != null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool isGrounded = IsGrounded();

        // Jump
        if (Input.GetButtonDown("Fire1"))
        {
            jumpBufferTimer = jumpBufferDuration;
        }
        else if (Input.GetButton("Fire1"))
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            jumpBufferTimer = 0.0f;
        }

        if ((jumpBufferTimer > 0.0f || coyoteTime > 0.0f) && (isGrounded || isClimbing))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpBufferTimer = 0.0f;
            coyoteTime = 0.0f;
        }
        else if (!isGrounded && !isClimbing)
        {
            coyoteTime -= Time.deltaTime;
        }
        // Dash
        if (Input.GetButtonDown("Fire2"))
        {
            if (dashCoolDownTimer <= 0.0f && !isDashing && (!hasDashedAfterJump || isGrounded))
            {
                isDashing = true;
                dashTimer = dashDuration;
                dashCoolDownTimer = dashCooldown;

                Vector2 dashDirection = new Vector2(horizontalInput, verticalInput).normalized;
                if (dashDirection.sqrMagnitude > 0.0f)
                {
                    rb.velocity = dashDirection * dashForce;
                }
                else
                {
                    rb.velocity = isFacingRight ? Vector2.right * dashForce : Vector2.left * dashForce;
                }

                if (!IsGrounded())
                {
                    hasDashedAfterJump = true;
                }
            }
        }

        // Update dash
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0.0f)
            {
                isDashing = false;
            }
        }

        // Update dash cooldown
        if (dashCoolDownTimer > 0.0f)
        {
            dashCoolDownTimer -= Time.deltaTime;
        }

        // Restore Dash
        if(hasDashedAfterJump && isGrounded){
            hasDashedAfterJump = false;
        }

        // Wall climb
        if (IsTouchingWall()  && Input.GetButton("Fire3"))
        {
            isClimbing = true;
            if(stamina > 0.0f){
                rb.velocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed);
                stamina -= staminaDrainRate * Time.deltaTime;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
        else
        {
            isClimbing = false;
        }

        // Recharge stamina
        if (IsGrounded())
        {
            stamina = Mathf.Min(stamina + staminaRechargeRate * Time.deltaTime, 100.0f);
        }

        // Wall jump
        if (IsTouchingWall() && Input.GetButtonDown("Fire1"))
        {
            wallJumpDirection = (isFacingRight ? Vector2.left : Vector2.right) * moveSpeed;
            rb.velocity = new Vector2(wallJumpDirection.x, jumpForce);
            isClimbing = false;
        }

                // Update animator
        if(animator != null)
        {
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("yVelocity", rb.velocity.y);
            animator.SetBool("isClimbing", isClimbing);
            animator.SetBool("isDashing", isDashing);
        }

        // Movement
        if (!isDashing)
        {
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(horizontalInput * dashForce, rb.velocity.y), dashDirectionSmoothness * Time.deltaTime);
        }

        // Update facing direction
        if (horizontalInput > 0.0f)
        {
            isFacingRight = true;
        }
        else if (horizontalInput < 0.0f)
        {
            isFacingRight = false;
        }

        // Update sprite
        if (isFacingRight)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }

    bool IsGrounded()
    {
        Vector2 bottomCenter = new Vector2(transform.position.x, transform.position.y - 0.5f);
        return Physics2D.Raycast(bottomCenter, Vector2.down, collider.bounds.extents.y + 0.1f, groundLayer);
    }

    bool IsTouchingWall()
    {   
        Vector2 bottomCenter = new Vector2(transform.position.x, transform.position.y - 0.5f);
        return Physics2D.Raycast(bottomCenter, isFacingRight ? Vector2.right : Vector2.left, collider.bounds.extents.x + 0.1f, groundLayer);
    }
}