using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcMoveScript : MonoBehaviour
{
    public float speed;
    [HideInInspector] public float initSpeed;
    public float jumpSpeed;
    public float climbSpeed;

    public float range;
    public float sight;
    public float wallBuffer;
    public bool engaged;
    public bool inRange;
    public bool moving;
    bool facingRight = false;

    public bool canJump = false;
    public bool canFly = false;
    public bool canClimb = false;
    public bool detectPlatformEdges = true;
    public bool wanders = true;
    int moveDir = 0;

    public List<GameObject> targets;
    public GameObject currentTarget;
    public GameObject defaultTarget;

    public float avgWanderTime = 2f;
    public float variationFactor = 3f;

    float timer = 0f;

    public string targetTag;

    public Transform groundDetection;
    public bool grounded = true;

    public Vector3 vel;
    Rigidbody2D rb;
    Damagable dmgScript;

    public Collider2D bodyCollider;
    public Collider2D footCollider;
    public LayerMask groundLayer;
    public LayerMask climbLayer;

    public bool attackCooldown = false;

    public Transform attackPoint;
    public Transform wallCheckpoint;

    npcAttackScript npc;

    public Animator height;

    // Start is called before the first frame update
    void Start()
    {
        initSpeed = speed;
        targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(targetTag));
        if (defaultTarget != null)
            targets.Add(defaultTarget);
        SetTarget();
        rb = GetComponent<Rigidbody2D>();
        if (canFly)
            rb.gravityScale = 0;
        npc = GetComponent<npcAttackScript>();
        dmgScript = GetComponent<Damagable>();

        if (footCollider == null && bodyCollider != null){
            footCollider = bodyCollider;
        }

        if(wallCheckpoint == null) wallCheckpoint = attackPoint;
        
    }
    // Update is called once per frame
    void Update()
    {   
        if(!facingRight){
            if(transform.eulerAngles.z < -30f){
                transform.eulerAngles = new Vector3(0f, 0f, -30f);
            }else if(transform.eulerAngles.z > 30f){
                transform.eulerAngles = new Vector3(0f, 0f, 30f);
            }
        } else {
            if(transform.eulerAngles.z < -30f){
                transform.eulerAngles = new Vector3(0f, 180f, -30f);
            }else if(transform.eulerAngles.z > 30f){
                transform.eulerAngles = new Vector3(0f, 180f, 30f);
            }
        }
        
        
        
        if(!(targets.Contains(GameObject.Find("Player"))) && gameObject.tag == "Enemy")
        {
            targets.Add(GameObject.Find("Player"));
            SetTarget();
        }
        timer += Time.deltaTime;

        if(!grounded) vel.y = rb.velocity.y;
        InspectTarget();

        if (engaged)
        {
            Engage();
        } else {
            if (wanders)
            {
                CalculateWander();
            } else
            {
                Patrol();
            }
            
        }

        if (canFly)
            CalibrateFlight();
        if (canClimb)
            CalculateClimb();

        CheckGrounding();

        
        if(grounded) {
            rb.rotation = 0f;
            rb.angularVelocity = 0f;
        }
        if(!dmgScript.knockedBack)
            rb.velocity = vel;
    }

    void Flip()
    {
        if (facingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingRight = true;
        }
    }

    void Idle()
    {
        moving = false;
        vel.x = 0;
        if (canFly)
        {
            vel.y = 0;
        }
            
    }

    void Move()
    {
        moving = true;
        if (facingRight)
        {
            vel.x = speed;
        }
        else
        {
            vel.x = -speed;
        }       
    }

    void Patrol()
    {
        if(canFly){
            vel.y = 0;
        }
                
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.1f, groundLayer);
        bool wallInfo = Physics2D.OverlapCircle(wallCheckpoint.position, wallBuffer, groundLayer);

        if ((groundInfo.collider == null && detectPlatformEdges && grounded) || wallInfo)
        {
            Flip();
        }
        Move();
    }

    void Engage()
    {
        if (!inRange)
        {
            Move();
            if (canJump && !canFly)
            {
                CalculateJump();
            } else if (canFly)
            {
                if(currentTarget.transform.position.y - transform.position.y > 0.3f)
                {
                    vel.y = speed;
                } else if (currentTarget.transform.position.y - transform.position.y < -0.3f)
                {
                    vel.y = -speed;
                }
            }
                
        }
        else
        {
            Idle();
        }

        if ((facingRight && currentTarget.transform.position.x - transform.position.x < -0.3f) ||
        (!facingRight && currentTarget.transform.position.x - transform.position.x > 0.3f))
        {
            Flip();
        }

    }

    void CheckGrounding()
    {
        if (footCollider.IsTouchingLayers(groundLayer))
        {
            if(height != null && !grounded){
                height.SetTrigger("squash");
            }
            grounded = true;
            
        }
        else
        {
            if(height != null && grounded){
                height.SetTrigger("stretch");
            }
            grounded = false;
        }
    }

    void CalibrateFlight()
    {
        if (Random.Range(0, 10f) < variationFactor / 10)
        {
            float tempRand = Random.Range(0, 1f);
            if (tempRand > 0.66f)
            {
                moveDir = 0;
            }
            else if (tempRand > 0.33f)
            {
                moveDir = 1;
            }
            else if (tempRand > 0)
            {
                moveDir = 2;
            }
        }
        if (bodyCollider.IsTouchingLayers(groundLayer))
        {
            float tempRand = Random.Range(0, 1f);
            if (tempRand > 0.5f)
            {
                if (moveDir == 0)
                {
                    moveDir = 1;
                }
                else if (moveDir == 1)
                {
                    moveDir = 2;
                }
                else if (moveDir == 2)
                {
                    moveDir = 0;
                }

            }
            else
            {
                if (moveDir == 0)
                {
                    moveDir = 2;
                }
                else if (moveDir == 1)
                {
                    moveDir = 0;
                }
                else if (moveDir == 2)
                {
                    moveDir = 1;
                }
            }
        }
    }

    void CalculateWander()
    {
        if (moving)
        {
            if(canFly){
                vel.y = 0;
            }
                
            CalibrateFlightDir();

            VariateMovement("flip");
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.2f, groundLayer);
            bool wallInfo = Physics2D.OverlapCircle(wallCheckpoint.position, wallBuffer, groundLayer);
            Collider2D col = Physics2D.OverlapCircle(wallCheckpoint.position, wallBuffer, groundLayer);
            
            if ((groundInfo.collider == null && detectPlatformEdges && grounded) || wallInfo)
            {
                if(groundInfo.collider == null && detectPlatformEdges && grounded) Debug.Log("Flipped on lack of ground.");
                if(col != null) Debug.Log("Wanderer flipped on wall on the following: " + col.gameObject.name);
                Flip();
            }
            VariateMovement("idle");
        }
        else
        {
            VariateMovement("move");
        }
    }

    void InspectTarget()
    {
        SetTarget();
        if (currentTarget != null)
        {
            engaged = CheckTargetDist(currentTarget, sight);
            inRange = CheckTargetDist(currentTarget, range);
        }
        else
        {
            engaged = false;
        }

        targets = new List<GameObject>(GameObject.FindGameObjectsWithTag(targetTag));
        if (defaultTarget != null)
            targets.Add(defaultTarget);
        SetTarget();
    }

    void CalculateJump()
    {
        if (currentTarget.transform.position.y - transform.position.y > 1.5f && grounded && !attackCooldown)
        {
            vel.y = jumpSpeed;
        }
    }

    void CalculateClimb()
    {
        if (bodyCollider.IsTouchingLayers(climbLayer))
        {
            if (currentTarget.transform.position.y - transform.position.y > 1.5f &&
                !grounded && canJump && !canFly && engaged && !inRange)
            {
                vel.y = climbSpeed;
            }
        }
    }

    void CalibrateFlightDir()
    {
        if (!canFly || moveDir == 0)
        {
            Move();
        }
        else if (canFly && moveDir == 1)
        {
            vel.y = speed;
        }
        else if (canFly && moveDir == 2)
        {
            vel.y = -speed;
        }
    }

    void VariateMovement(string type)
    {
        if (timer > avgWanderTime * Random.Range(1 / variationFactor, variationFactor))
        {
            if (type == "flip")
            {
                Debug.Log("Wanderer flipped on variate.");
                Flip();
            }
            else if (type == "idle")
            {
                Idle();
            }
            else if (type == "move")
            {
                Move();
            }
            timer = 0f;
        }
    }

    void SetTarget()
    {
        if (targets.Count > 0)
        {
            foreach (GameObject target in targets)
            {
                if (target == null)
                {
                    targets.Remove(target);
                    SetTarget();
                } else
                {
                    if (currentTarget == null || Vector3.Distance(target.transform.position, attackPoint.transform.position)
                        <= Vector3.Distance(currentTarget.transform.position, attackPoint.transform.position))
                    {
                        currentTarget = target;
                    }
                }
                    
                
            }
        }
    }

    bool CheckTargetDist(GameObject target, float dist)
    {
        if (Vector2.Distance(new Vector2(target.transform.position.x - 0.65f * Mathf.Round((target.transform.position-attackPoint.transform.position).normalized.x), target.transform.position.y), attackPoint.transform.position) <= dist)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, range);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallCheckpoint.position, wallBuffer);
        Gizmos.color = Color.white;
        Vector3 sightDist = new Vector3(attackPoint.position.x - sight, attackPoint.position.y, attackPoint.position.z);
        Vector3 sightDist2 = new Vector3(attackPoint.position.x + sight, attackPoint.position.y, attackPoint.position.z);
        Vector3 sightDist3 = new Vector3(attackPoint.position.x, attackPoint.position.y - sight, attackPoint.position.z);
        Vector3 sightDist4 = new Vector3(attackPoint.position.x, attackPoint.position.y + sight, attackPoint.position.z);
        Vector3 groundDist = groundDetection.position;
        groundDist.y -= 0.5f;
        Gizmos.DrawLine(groundDetection.position, groundDist);
        Gizmos.DrawLine(attackPoint.position, sightDist);
        Gizmos.DrawLine(attackPoint.position, sightDist2);
        Gizmos.DrawLine(attackPoint.position, sightDist3);
        Gizmos.DrawLine(attackPoint.position, sightDist4);
    }
}
