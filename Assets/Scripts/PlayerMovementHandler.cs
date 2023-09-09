using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    // Feature Refs

    public Rigidbody2D rb;  // Reference to the Rigidbody2D component
    DamageHandler dmgScript;  // Reference to the DamageHandler script
    Transform t;  // Reference to the Transform component

    public Collider2D bodyCollider, wallCollider;  // Reference to the colliders on the character's body
    public LayerMask groundLayer;  // Layer mask to determine the ground layer

    GameObject Cam;  // Reference to the main CameraManager

    public ParticleSystem Dust;  // Reference to the particle system for dust effects
    public AudioSource JumpAudio, DoubleJumpAudio;  // Reference to the audio sources for jump sounds
    public Animator heightAnim;  // Reference to the animator for height animation
    
    // Public variables

    public bool isGrounded = false;  // A flag to check if the character is grounded
    public bool isGroundedDelay = false;  // A flag to check if the character is grounded
    public float walkSpeed = 12f;  // The maximum walking speed of the character

    public float jumpSpeed;  // The speed at which the character jumps
    public static int level = 1;  // A static variable representing the current level

    public static float direction;  // The current movement direction of the character
    public bool movingRight;  // A flag to check if the character is moving to the right

    //Coyote Time - The duration in seconds after leaving a ground where the character can still jump
    // Buffer Time - The duration in seconds to buffer the jump input
    public float coyoteTime, bufferTime = 0.3f;  // 
    float coyoteCounter, bufferCounter;  // Counters for coyote time and bufferTime

    bool doublejump;  // A flag to check if the character can perform a double jump

    

    // canDecelerate - A flag to check if the character can decelerate
    // decelerating - A flag to check if the character is currently decelerating
    // onWall - A flag to check if the character is currently on a wall
    bool canDecelerate, decelerating = false;
    public bool onWall = false;

    void Start()
    {
        // Initialize references to components and objects
        rb = GetComponent<Rigidbody2D>();
        dmgScript = GetComponent<DamageHandler>();
        t = transform;
        movingRight = t.eulerAngles.y > -180;
        Cam = Camera.main.gameObject;
    }

    void Update()
    {
        float x_input = Input.GetAxis("Horizontal");

        // Check if the character is touching a wall
        onWall = wallCollider.IsTouchingLayers(groundLayer);

        // Update character's horizontal velocity based on input
        if (Mathf.Abs(x_input) > 0 && !decelerating)
            rb.velocity = new Vector2(x_input * walkSpeed, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);

        // Handle Deceleration

        // Check if the character can decelerate or is currently decelerating
        if (Mathf.Abs(x_input) == 1) {
            canDecelerate = true;
        }
        else if (Mathf.Abs(x_input) == 0) {
            canDecelerate = false;
            decelerating = false;
        } else if (Mathf.Abs(x_input) < 0.1 && canDecelerate)
        {
            decelerating = true;
        }

        //Handle Direction


        // Update character's movement direction
        direction = rb.velocity.x;

        // Flip the character's sprite based on movement direction
        if ((movingRight && direction <= -0.1) || (!movingRight && direction >= 0.1)) {
            Flip();
        }

        // Handle character's grounded state and coyote time

        // Check if the character's feet are touching the ground
        isGrounded = bodyCollider.IsTouchingLayers(groundLayer);

        if (isGrounded)
        {
            // Check if the character has stopped moving and is grounded
            if (rb.velocity.x == 0) decelerating = false;
            // Adjust character's rotation when grounded
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

            if (!isGroundedDelay)
            {
                heightAnim.SetTrigger("squash");
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }
            coyoteCounter = coyoteTime;
            doublejump = true;
        }
        else
        {
            coyoteCounter -= Time.deltaTime;
        }

        isGroundedDelay = isGrounded;  // Update character's grounded state

        // Buffer jump input for a certain duration after leaving the ground
        bufferCounter = Input.GetButtonDown("Jump") ? bufferTime : bufferCounter - Time.deltaTime;

        // Perform buffered jump if within buffer time and coyote time
        if (bufferCounter > 0 && coyoteCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            bufferCounter = 0;
            CreateDust();
            JumpAudio.Play();
        }

        // Perform double jump if jump input is detected and character can double jump
        if (Input.GetButtonDown("Jump") && CanDoubleJump()) {
            Jump();
            doublejump = false;
        }

        // Reduce upward velocity if jump input is released before reaching the peak of the jump
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteCounter = 0;
        }

        // Apply knockback if the character is being knocked back
        if (dmgScript.knockedBack) dmgScript.applyKnock();

    }

    // Perform a jump
    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        bufferCounter = 0;
        CreateDust();
        JumpAudio.Play();
    }

    // Check if the character can perform a double jump
    bool CanDoubleJump()
    {
        return (!isGrounded && doublejump == true);
    }

    // Flip the character's sprite horizontally
    void Flip()
    {
        CreateDust();
        t.eulerAngles = new Vector3(0, movingRight ? -180 : 0, 0);
        movingRight = !movingRight;
    }

    // Create dust particle effects
    void CreateDust()
    {
        Dust.Play();
    }
}
