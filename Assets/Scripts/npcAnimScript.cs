using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcAnimScript : MonoBehaviour
{
    Animator anim;
    npcMoveScript npcMove;
    npcAttackScript npcAttack;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if(parent == null){
            npcMove = GetComponent<npcMoveScript>();
            npcAttack = GetComponent<npcAttackScript>();
        } else {
            npcMove = parent.GetComponent<npcMoveScript>();
            npcAttack = parent.GetComponent<npcAttackScript>();
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
