using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{

    public Rigidbody2D rb;
    DamageHandler dmgScript;
    Transform t;

    public Collider2D bodyCollider, wallCollider;
    public LayerMask groundLayer;

    public ParticleSystem Dust;
    public AudioSource JumpAudio, DoubleJumpAudio;
    public Animator heightAnim;
    
    // Public variables

    public bool isGrounded = false;
    public bool isGroundedDelay = false;
    public float walkSpeed = 12f;

    public float jumpSpeed;

    public static float direction;
    public bool movingRight;

    public float coyoteTime, bufferTime = 0.3f;
    float coyoteCounter, bufferCounter;

    public bool doublejump;

    bool canDecelerate, decelerating = false;
    public bool onWall = false;

    void Start()
    {
        // Initialize references to components and objects
        rb = GetComponent<Rigidbody2D>();
        dmgScript = GetComponent<DamageHandler>();
        t = transform;
        movingRight = t.eulerAngles.y > -180;
    }

    void Update()
    {
        float x_input = Input.GetAxis("Horizontal");

        onWall = wallCollider.IsTouchingLayers(groundLayer);
        isGrounded = bodyCollider.IsTouchingLayers(groundLayer);

        if (Mathf.Abs(x_input) > 0 && !decelerating)
            rb.velocity = new Vector2(x_input * walkSpeed, rb.velocity.y);
        else {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        direction = rb.velocity.x;

        if (Mathf.Abs(x_input) == 1) {
            canDecelerate = true;
        } else if (Mathf.Abs(x_input) == 0) {
            canDecelerate = false;
            decelerating = false;
        } else if (Mathf.Abs(x_input) < 0.1 && canDecelerate) {
            decelerating = true;
        }
        

        if ((movingRight && direction <= -0.1) || (!movingRight && direction >= 0.1)) {
            Flip();
        }

        if (isGrounded)
        {
            if (rb.velocity.x == 0) {
                decelerating = false;
            }

            if (!isGroundedDelay) {
                heightAnim.SetTrigger("squash");
            }

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

            coyoteCounter = coyoteTime;
            doublejump = true;

        } else {
            coyoteCounter -= Time.deltaTime;
        }

        isGroundedDelay = isGrounded;

        if (Input.GetButtonDown("Jump")) {
            bufferCounter = bufferTime;
            Jump();
        } else {
            bufferCounter -= Time.deltaTime;
        }

        if (dmgScript.knockedBack) {
            dmgScript.applyKnock();
        }

    }

    public void Jump()
    {
        if ((bufferCounter > 0 && coyoteCounter > 0) || CanDoubleJump()){
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            bufferCounter = 0;
            Dust.Play();
            JumpAudio.Play();
            doublejump = false;
        } else if (rb.velocity.y > 0){
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteCounter = 0;
        }
    }

    bool CanDoubleJump()
    {
        return (!isGrounded && doublejump == true);
    }

    void Flip()
    {
        Dust.Play();
        t.eulerAngles = new Vector3(0, movingRight ? -180 : 0, 0);
        movingRight = !movingRight;
    }
}
