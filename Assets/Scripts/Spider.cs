using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    bool sleeping = true;
    Rigidbody2D rb;
    EnemyMovementHandler moveScript;
    EnemyAttackHandler attackScript;
    DamageHandler damageScript;

    public Collider2D dropTrigger;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackScript = GetComponent<EnemyAttackHandler>();
        moveScript = GetComponent<EnemyMovementHandler>();
        damageScript = GetComponent<DamageHandler>();

        rb.bodyType = RigidbodyType2D.Static;
        attackScript.enabled = false;
        moveScript.enabled = false;
        damageScript.enabled = false;
    }

    public void Drop(){
        dropTrigger.enabled = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        attackScript.enabled = true;
        moveScript.enabled = true;
        damageScript.enabled = true;
        sleeping = false;
    }

    public void Jump(){
        if(moveScript.grounded) moveScript.vel.y = moveScript.jumpSpeed;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Player"){
            Drop();
        }
    }
}
