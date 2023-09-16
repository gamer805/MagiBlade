using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{

    [HideInInspector] public Rigidbody2D rb;
    DamageHandler damageHandler;

    public Collider2D bodyCollider, wallCollider;
    public LayerMask groundLayer, climbLayer;

    public ParticleSystem Dust;
    public AudioSource JumpAudio;
    public GameObject Weapon;
    

    public bool isGrounded = false;
    public bool onWall = false;
    public bool onLadder = false;

    public float walkSpeed = 12f;
    public float jumpSpeed;
    public float climbSpeed = 8f;
    float gScale;

    public static float direction;
    public bool movingRight;

    public float coyoteTime, bufferTime = 0.3f;
    float coyoteCounter, bufferCounter;
    bool shortenJump = false;

    public bool canDoubleJump;
    public bool isClimbing;

    bool canDecelerate, decelerating = false;
    

    void Start()
    {
        // Initialize references to components and objects
        rb = GetComponent<Rigidbody2D>();
        damageHandler = GetComponent<DamageHandler>();
        gScale = rb.gravityScale;
        movingRight = transform.eulerAngles.y > -180;
    }

    void Update() {

        onWall = wallCollider.IsTouchingLayers(groundLayer);
        isGrounded = bodyCollider.IsTouchingLayers(groundLayer);
        onLadder = bodyCollider.IsTouchingLayers(climbLayer);

        bufferCounter -= Time.deltaTime;
        if (Input.GetButtonDown("Jump")) {
            if (!isGrounded && canDoubleJump) {
                canDoubleJump = false;
                Jump();
            } else {
                bufferCounter = bufferTime;
            }
        }
        if (isGrounded) {
            coyoteCounter = coyoteTime;
        } else {
            coyoteCounter -= Time.deltaTime;
        }
        if (bufferCounter > 0 && coyoteCounter > 0){
            bufferCounter = 0;
            coyoteCounter = 0;
            canDoubleJump = true;
            Jump();
        }
        
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0) {
            shortenJump = true;
        }
        
    }

    void FixedUpdate()
    {
        float x_input = Input.GetAxis("Horizontal");
        float y_input = Input.GetAxis("Vertical");

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

        if (!isClimbing && ((movingRight && direction <= -0.1) || (!movingRight && direction >= 0.1))) {
            Flip();
        }

        if (isGrounded) {

            if (rb.velocity.x == 0) {
                decelerating = false;
            }

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }

        if(onLadder){
            if (!isClimbing) {
                isClimbing = true;
                Weapon.SetActive(false);
                rb.velocity = Vector2.zero;
            } else {
                rb.velocity = new Vector2(x_input * climbSpeed * 0.25f, y_input*climbSpeed);
                rb.gravityScale = 0f;
            }
        } else {
            if (isClimbing) {
                isClimbing = false;
                rb.gravityScale = gScale;
                Weapon.SetActive(true);
            }
            
        }

        if (shortenJump) {
            shortenJump = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

    }

    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        Dust.Play();
        JumpAudio.Play();
    }

    void Flip()
    {
        Dust.Play();
        transform.eulerAngles = new Vector3(0, movingRight ? -180 : 0, 0);
        movingRight = !movingRight;
    }
}
