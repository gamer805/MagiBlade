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

    enum Direction {
        HORIZONTAL,
        UP,
        DOWN
    }

    Direction movementDirection;

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
        } else if (canWander) {
            if (canFly) {
                Float();
            } else {
                Wander();
            }
        } else {
            if (canFly) currentVelocity.y = 0f;
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

    void Patrol() {

        if ((onLedge && detectsLedges && isGrounded) || onWall) {
            Flip();
        }
        currentVelocity.x = facingRight ? walkSpeed : -walkSpeed;

    }

    void Wander() {

        bool canDecide = decisionTimer > decisionRate && Random.Range(0f, 1f) > decisionThreshold;
        if (isMoving) {
            Patrol();
            if (canDecide) {
                decisionTimer = 0f;
                MakeDecision();
            }
        } else if (canDecide) {
            decisionTimer = 0f;
            isMoving = true;
            currentVelocity.x = facingRight ? walkSpeed : -walkSpeed;
        }
    }

    void Float() {

        bool canDecide = decisionTimer > decisionRate && Random.Range(0f, 1f) > decisionThreshold;
        if (isMoving) {

            if (movementDirection == Direction.HORIZONTAL) {
                currentVelocity = new Vector2(facingRight ? walkSpeed : -walkSpeed, 0);
            } else if (movementDirection == Direction.UP) {
                currentVelocity.y = walkSpeed;
            } else if (movementDirection == Direction.DOWN) {
                currentVelocity.y = -walkSpeed;
            }

            if (canDecide) {
                decisionTimer = 0f;
                movementDirection = (Direction) Random.Range(0, 3);
                MakeDecision();
            }

        } else if (canDecide) {
            decisionTimer = 0f;
            isMoving = true;
            currentVelocity.x = facingRight ? walkSpeed : -walkSpeed;
        }
        

        if (onWall) {
            if (movementDirection == Direction.UP) {
                movementDirection = Direction.DOWN;
            } else if (movementDirection == Direction.DOWN) {
                movementDirection = Direction.UP;
            } else {
                Flip();
            }
        }
    }


    void Engage()  {
        
        Vector2 targetDistance = playerRef.transform.position - transform.position;

        if (!inRange) {
            isMoving = true;
            currentVelocity.x = facingRight ? walkSpeed : -walkSpeed;
            if (canFly) {
                currentVelocity.y = targetDistance.y > 0f ? walkSpeed : -walkSpeed;
            } else if (targetDistance.y > 1.5f && !attackHandler.isCoolingDown) {
                if (canJump && isGrounded && !hasJumped) {
                    rb.velocity = new Vector2(0, jumpSpeed);
                    hasJumped = true;
                } else if (canClimb && onLadder) {
                    rb.velocity = new Vector2(0, climbSpeed);
                }
            }
        } else {
            isMoving = false;
            currentVelocity.x = 0;
            if (canFly) currentVelocity.y = 0;
        }

        if ((facingRight && targetDistance.x < -0.3f) || (!facingRight && targetDistance.x > 0.3f)) {
            Flip();
        }

    }

    void MakeDecision () {
        if (Random.Range(0, 1f) > 0.5f) {
            Flip();
        } else {
            isMoving = false;
            currentVelocity = Vector2.zero;
        }
    }
}
