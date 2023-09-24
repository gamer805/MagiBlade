using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    PlayerMovementHandler movementData;
    public GameObject parent;

    void Start() {
        if (parent == null) {
            rb = GetComponent<Rigidbody2D>();
            movementData = GetComponent<PlayerMovementHandler>();
        } else {
            
            rb = parent.GetComponent<Rigidbody2D>();
            movementData = parent.GetComponent<PlayerMovementHandler>();
        }
        anim = GetComponent<Animator>();
    }

    void FixedUpdate() {
        if(!movementData.onWall) {
            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }
        anim.SetFloat("ClimbSpeed", Mathf.Abs(rb.velocity.y));

        anim.SetBool("IsJumping", !movementData.isGrounded);
        anim.SetBool("IsClimbing", movementData.isClimbing);
    }
}
