using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    Animator anim; // Animator component for the character
    Rigidbody2D rb; // Rigidbody2D component for the character
    PlayerMovementHandler movementData; // PlayerMovementHandler script for the character

    // Reference to the character's parent GameObject, used to access the character's components if the script is attached to a child object
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        if (parent == null) {
            rb = GetComponent<Rigidbody2D>();
            movementData = GetComponent<PlayerMovementHandler>();
        } else {
            
            rb = parent.GetComponent<Rigidbody2D>();
            movementData = parent.GetComponent<PlayerMovementHandler>();
        }
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(!movementData.onWall) {
            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        }
        anim.SetFloat("ClimbSpeed", Mathf.Abs(rb.velocity.y));

        anim.SetBool("IsJumping", !movementData.isGrounded);
        anim.SetBool("IsClimbing", movementData.isClimbing);
    }
}
