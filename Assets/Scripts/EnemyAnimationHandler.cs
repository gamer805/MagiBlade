using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    Animator anim;
    EnemyMovementHandler movementData;
    EnemyAttackHandler attackData;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if(parent == null){
            movementData = GetComponent<EnemyMovementHandler>();
            attackData = GetComponent<EnemyAttackHandler>();
        } else {
            movementData = parent.GetComponent<EnemyMovementHandler>();
            attackData = parent.GetComponent<EnemyAttackHandler>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("IsJumping", !movementData.isGrounded);
        anim.SetFloat("Speed", Math.Abs(movementData.currentVelocity.x));
        if(attackData.animateAttack == true && attackData.isAttacking == true){
            attackData.isAttacking = false;
            anim.SetTrigger("Attack");
        }
        
    }
}
