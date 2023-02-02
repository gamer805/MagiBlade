using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public bool groundingChecker = false;
    public float maxWalkSpeed = 12f;

    public float jumpSpeed;
    public static int level = 1;
    public bool grounded;
    public Rigidbody2D rb;
    Damagable dmgScript;

    public static float direction;
    Transform t;
    public bool movingRight;

    public Collider2D bodyCollider;
    public Collider2D footCollider;
    public Collider2D wallCollider;
    public LayerMask groundLayer;

    public float coyoteTime = 0.3f;
    float coyoteCounter;

    public float bufferTime = 0.2f;
    float bufferCounter;

    bool doublejump;

    GameObject Cam;

    public ParticleSystem Dust;
    public AudioSource JumpAudio;
    public AudioSource DoubleJumpAudio;
    public Animator heightAnim;

    bool canDecelerate = false;
    bool decelerating = false;

    public bool onWall = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dmgScript = GetComponent<Damagable>();
        t = transform;
        movingRight = t.eulerAngles.y > -180;
        Cam = Camera.main.gameObject;
    }

    void Update(){
        onWall = wallCollider.IsTouchingLayers(groundLayer);

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0 && !decelerating)
            rb.velocity = new Vector2(Input.GetAxis("Horizontal")*maxWalkSpeed, rb.velocity.y);
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if(Mathf.Abs(Input.GetAxis("Horizontal")) == 1){
            canDecelerate = true;
        }
        if(Mathf.Abs(Input.GetAxis("Horizontal")) == 0){
            canDecelerate = false;
            decelerating = false;
        }
        if(rb.velocity.x == 0 && IsGrounded()){
            decelerating = false;
        }
        if(Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1 && canDecelerate){
            decelerating = true;
        }


        if((IsGrounded() || bodyCollider.IsTouchingLayers(groundLayer))){
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }

        direction = rb.velocity.x;
        
        if(direction <= -0.1 || direction >= 0.1)
        {
            if ((movingRight && direction < 0) || (!movingRight && direction > 0))
                Flip();
        }

        if(IsGrounded()){
            if(!grounded){
                heightAnim.SetTrigger("squash");
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            }
            coyoteCounter = coyoteTime;
            doublejump = true;
        } else {
            coyoteCounter -= Time.deltaTime;
        }

        grounded = IsGrounded();

        if(Input.GetButtonDown("Jump")){
            bufferCounter = bufferTime;
        } else {
            bufferCounter -= Time.deltaTime;
        }

        if(bufferCounter > 0 && coyoteCounter > 0){
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            bufferCounter = 0;
            CreateDust();
            JumpAudio.Play();
        }
        if(Input.GetButtonDown("Jump") && CanDoubleJump()){
            doublejump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            CreateDust();
            DoubleJumpAudio.Play();
        }

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0){
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteCounter = 0;
        }
        

        if(dmgScript.knockedBack){
            dmgScript.applyKnock();
        }
    }

    void FixedUpdate(){
        
        groundingChecker = footCollider.IsTouchingLayers(groundLayer);
        
    }
    bool IsGrounded(){
        return groundingChecker;
    }

    public void Jump(){
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        bufferCounter = 0;
        CreateDust();
        JumpAudio.Play();
    }
    

    bool CanDoubleJump(){
        if(!IsGrounded() && doublejump == true){
            return true;
        } else {
            return false;
        }
    }

    void Flip()
    {
        CreateDust();
        if (movingRight)
        {
            t.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
        }
        else
        {
            t.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
        }
    }



    void CreateDust(){
        Dust.Play();
    }
}
