using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDamageHandler : MonoBehaviour
{
    public float attackDamage = 10f;
    public bool contacting = false;

    PlayerMovementHandler playerMovementHandler;
    DamageHandler damageHandler;
    void Start() {
        playerMovementHandler = GameObject.Find("Player").GetComponent<PlayerMovementHandler>();
        damageHandler = gameObject.transform.parent.gameObject.GetComponent<DamageHandler>();
    }
    void OnCollisionEnter2D(Collision2D col){
        if(!contacting && col.gameObject.tag == "PlayerFoot"){
            contacting = true;
            playerMovementHandler.Jump();
            playerMovementHandler.canDoubleJump = true;

            //damageHandler.ApplyDamage(attackDamage, col.gameObject, playerMovementHandler.jumpPower);
        }
    }

    void OnCollisionExit2D(Collision2D col){
        if(contacting && col.gameObject.tag == "PlayerFoot"){
            contacting = false;
        }
    }
}
