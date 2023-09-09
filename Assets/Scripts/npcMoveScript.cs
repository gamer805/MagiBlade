using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcMoveScript : MonoBehaviour
{
    public float speed;
    [HideInInspector] public float initSpeed;
    public float jumpSpeed;
    public float climbSpeed;

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

    public Collider2D sightRange;
    public Collider2D attackRange;
    public List<GameObject> targets;
    public List<Collider2D> targetCols;
    public GameObject currentTarget;

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
    public LayerMask groundLayer;
    public LayerMask climbLayer;

    public bool attackCooldown = false;

    public Transform attackPoint;
    public Transform wallCheckpoint;

    public Animator height;

    // Start is called before the first frame update
    void Start()
    {
        initSpeed = speed;
        SetTarget();
        rb = GetComponent<Rigidbody2D>();
        if (canFly) rb.gravityScale = 0;

        dmgScript = GetComponent<Damagable>();

        if(wallCheckpoint == null) wallCheckpoint = attackPoint;
        
    }
    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;

        if(!grounded) vel.y = rb.velocity.y;
        InspectTarget();

        if (engaged)
        {
            Engage();
        } else {
            if (wanders)
            {
                Wander();
            } else
            {
                Patrol();
            }
            
        }

        if (canClimb && bodyCollider.IsTouchingLayers(climbLayer))
        {
            if (currentTarget.transform.position.y - transform.position.y > 1.5f &&
                !grounded && canJump && !canFly && engaged && !inRange)
            {
                vel.y = climbSpeed;
            }
        }

        if (bodyCollider.IsTouchingLayers(groundLayer))
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
            ManageFlight();
        }
                
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.25f, groundLayer);
        bool wallInfo = Physics2D.OverlapCircle(wallCheckpoint.position, wallBuffer, groundLayer);

        if ((groundInfo.collider == null && detectPlatformEdges && grounded) || wallInfo)
        {
            Debug.Log("flip");
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
                Jump();
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

    void ManageFlight()
    {
        if (Random.Range(0, 10f) < variationFactor / 10)
        {
            moveDir = Random.Range(0, 3);
        }
        if (bodyCollider.IsTouchingLayers(groundLayer))
        {
            float tempRand = Random.Range(0, 1f);
            if (tempRand > 0.5f)
            {
                moveDir += 1;
                if (moveDir == 3) moveDir = 0;
            }
            else
            {
                moveDir -= 1;
                if (moveDir == -1) moveDir = 2;
            }
        }
    }

    void Wander()
    {
        if (moving)
        {
            if(canFly){
                ManageFlight();
                if (moveDir == 0)
                {
                    vel.y = 0;
                    Move();
                } else if (moveDir == 1) {
                    vel.y = speed;
                } else if (moveDir == 2) {
                    vel.y = -speed;
                }
            } else {
                Move();
            }

            if (canShift()) {
                Flip();
                timer = 0f;
            }
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.2f, groundLayer);
            bool wallInfo = Physics2D.OverlapCircle(wallCheckpoint.position, wallBuffer, groundLayer);
            Collider2D col = Physics2D.OverlapCircle(wallCheckpoint.position, wallBuffer, groundLayer);
            
            if ((groundInfo.collider == null && detectPlatformEdges && grounded) || wallInfo)
            {
                Flip();
            }

            if (canShift()) {
                Idle();
                timer = 0f;
            }
        }
        else
        {
            if (canShift()) {
                Move();
                timer = 0f;
            }
        }
    }

    void InspectTarget()
    {
        SetTarget();
        if (currentTarget != null)
        {
            engaged = TargetInRange(currentTarget, sightRange);
            inRange = TargetInRange(currentTarget, attackRange);
        }
        else
        {
            engaged = false;
        }
    }

    void Jump()
    {
        if (currentTarget.transform.position.y - transform.position.y > 1.5f && grounded && !attackCooldown)
        {
            vel.y = jumpSpeed;
        }
    }

    void Climb()
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

    bool canShift()
    {
        return (timer > avgWanderTime * Random.Range(1 / variationFactor, variationFactor));
    }

    void SetTarget()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Player")){
            if(!targets.Contains(obj)) targets.Add(obj);
        }

        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Troop")){
            if(!targets.Contains(obj)) targets.Add(obj);
        }

        foreach(GameObject obj in targets){
            foreach(Collider2D col in obj.GetComponents<Collider2D>()){
                if(sightRange.IsTouching(col)){
                    if(!targetCols.Contains(col)) targetCols.Add(col);
                }
            }
        }

        foreach(GameObject obj in targets){
            if(currentTarget == null || GetTargetDistance(obj) < GetTargetDistance(currentTarget)){
                currentTarget = obj;
            }
        }
    }

    public float GetTargetDistance(GameObject target){
        return Vector2.Distance(new Vector2(target.transform.position.x - 0.65f * Mathf.Round((target.transform.position-attackPoint.transform.position).normalized.x), target.transform.position.y), attackPoint.transform.position);
    }

    bool TargetInRange(GameObject target, Collider2D col)
    {
        foreach(Collider2D targetCol in target.GetComponents<Collider2D>())  
        {
            if (col.IsTouching(targetCol)) return true;
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(wallCheckpoint.position, wallBuffer);
    }
}
