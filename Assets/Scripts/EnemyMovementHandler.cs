using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovementHandler : MonoBehaviour
{
    [HideInInspector] public float initialSpeed;
    public float walkSpeed;
    public float jumpSpeed;
    public float climbSpeed;

    public float wallBuffer;
    public bool inSight;
    public bool inRange;
    public bool isMoving;
    bool facingRight = false;

    public bool canJump = false;
    public bool canFly = false;
    public bool canClimb = false;
    public bool detectsLedges = true;
    public bool hasJumped = false;
    int movementDirection = 0;

    public Collider2D sightRange;
    public Collider2D attackRange;
    Collider2D targetCollider;
    GameObject playerRef;

    public bool canWander = true;
    public float decisionRate = 2f;
    public float decisionThreshold = 0.6f;
    float decisionTimer = 0f;

    public Vector3 currentVelocity;
    Rigidbody2D rb;
    DamageHandler damageHandler;
    EnemyAttackHandler attackHandler;

    Transform attackPoint;
    public Transform groundDetector;
    public Transform wallDetector;
    
    public Collider2D bodyCollider;
    public LayerMask groundLayer;
    public LayerMask climbLayer;

    bool onWall;
    bool onLedge;
    bool onLadder;
    public bool isGrounded = true;
    void Start() {
        initialSpeed = walkSpeed;
        playerRef = GameObject.Find("Player");
        targetCollider = playerRef.GetComponent<PlayerMovementHandler>().bodyCollider;
        rb = GetComponent<Rigidbody2D>();
        damageHandler = GetComponent<DamageHandler>();
        attackHandler = GetComponent<EnemyAttackHandler>();

        attackPoint = attackHandler.attackPoint;

        if (canFly) {
            rb.gravityScale = 0;
        }

        if(wallDetector == null) {
            wallDetector = attackPoint;
        }
        
    }

    void Update() {
        if (playerRef == null) {
            playerRef = GameObject.Find("Player");
        }
        onLedge = Physics2D.Raycast(groundDetector.position, Vector2.down, 0.2f, groundLayer).collider == null;
        onWall = Physics2D.OverlapCircle(wallDetector.position, wallBuffer, groundLayer);
        onLadder = bodyCollider.IsTouchingLayers(climbLayer);
        isGrounded = bodyCollider.IsTouchingLayers(groundLayer);

        inSight = targetCollider.IsTouching(sightRange);
        inRange = targetCollider.IsTouching(attackRange);

        decisionTimer += Time.deltaTime;
    }

    void FixedUpdate() {

        

        if (inSight && playerRef != null) {
            Engage();
        } else if (canFly) {
            Fly();
        } else if (canWander) {
            Wander();
        } else {
            Patrol();
        }
        
        if(isGrounded) {
            rb.rotation = 0f;
            rb.angularVelocity = 0f;
            if (hasJumped) {
                hasJumped = false;
            }
        }

        if(!damageHandler.knockedBack) {
            if (canFly) {
                rb.velocity = currentVelocity;
            } else {
                rb.velocity = new Vector2(currentVelocity.x, rb.velocity.y);
            }
        }
    }

    void Flip()
    {
        transform.eulerAngles = new Vector3(0, facingRight ? 0 : 180, 0);
        facingRight = !facingRight;
    }

    void Idle()
    {
        isMoving = false;
        currentVelocity.x = 0;
        if (canFly) {
            currentVelocity.y = 0;
        }
            
    }

    void Move() {
        isMoving = true;
        currentVelocity.x = facingRight ? walkSpeed : -walkSpeed; 
    }

    void Patrol() {

        if ((onLedge && detectsLedges && isGrounded) || onWall) {
            Flip();
        }
        Move();

    }

    void Wander() {

        bool canDecide = decisionTimer > decisionRate && Random.Range(0f, 1f) > decisionThreshold;
        if (isMoving) {
            Patrol();
            if (canDecide) {
                decisionTimer = 0f;
                if (Random.Range(0, 1f) > 0.5f) {
                    Flip();
                } else {
                    Idle();
                }
            }
        } else if (canDecide) {
            Move();
        }
    }

    void Engage()  {
        Vector2 targetDistance = transform.position - playerRef.transform.position;
        if (!inRange) {
            Move();
            if (canFly) {
                currentVelocity.y = targetDistance.y < 0f ? walkSpeed : -walkSpeed;
            } else if (canJump && targetDistance.y < -1.5f) {
                Jump();
            } else if (onLadder && canClimb && targetDistance.y < -1.5f) {
                Climb();
            }
        } else {
            Idle();
        }

        if ((facingRight && targetDistance.x > 0.3f) || (!facingRight && targetDistance.x < -0.3f)) {
            Flip();
        }

    }

    void Fly() {
        if (canWander) {

            bool canDecide = decisionTimer > decisionRate && Random.Range(0f, 1f) > decisionThreshold;
            if (canDecide) {
                movementDirection = Random.Range(0, 3);
                decisionTimer = 0f;
            }

            if (movementDirection == 0) {
                currentVelocity.y = 0;
                Move();
            } else if (movementDirection == 1) {
                currentVelocity.y = walkSpeed;
            } else {
                currentVelocity.y = -walkSpeed;
            }
            if (onWall) {
                if (Random.Range(0, 1f) > 0.5f) {
                    movementDirection += 1;
                    if (movementDirection == 3) movementDirection = 0;
                } else {
                    movementDirection -= 1;
                    if (movementDirection == -1) movementDirection = 2;
                }
            }
        } else {
            currentVelocity.y = 0;
            if ((onLedge && detectsLedges && isGrounded) || onWall) {
                Flip();
            }
            Move();
        }
    }

    void Jump() {
        if (isGrounded && !attackHandler.isCoolingDown && !hasJumped) {
            rb.velocity = new Vector2(currentVelocity.x, jumpSpeed);
            hasJumped = true;
        }
    }

    void Climb()
    {
        if (!isGrounded && !attackHandler.isCoolingDown) {
            rb.velocity = new Vector2(currentVelocity.x, climbSpeed);
        }
    }
}
