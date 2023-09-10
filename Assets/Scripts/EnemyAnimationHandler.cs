using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationHandler : MonoBehaviour
{
    Animator anim;
    EnemyMovementHandler npcMove;
    EnemyAttackHandler npcAttack;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if(parent == null){
            npcMove = GetComponent<EnemyMovementHandler>();
            npcAttack = GetComponent<EnemyAttackHandler>();
        } else {
            npcMove = parent.GetComponent<EnemyMovementHandler>();
            npcAttack = parent.GetComponent<EnemyAttackHandler>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("IsJumping", !npcMove.grounded);
        anim.SetFloat("Speed", Math.Abs(npcMove.vel.x));
        if(npcAttack.animateAttack == true && npcAttack.attacking == true){
            npcAttack.attacking = false;
            anim.SetTrigger("Attack");
        }
        
    }
}
