﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    Animator anim; // Animator component for the character
    Rigidbody2D rb; // Rigidbody2D component for the character
    PlayerMovementHandler character; // PlayerMovementHandler script for the character
    PlayerClimbingHandler climbData; // PlayerClimbingHandler for the character

    // Reference to the character's parent GameObject, used to access the character's components if the script is attached to a child object
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator, Rigidbody2D, PlayerMovementHandler, and PlayerClimbingHandler components from either the current object or its parent
        if (parent == null)
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            character = GetComponent<PlayerMovementHandler>();
            climbData = GetComponent<PlayerClimbingHandler>();
        }
        else
        {
            anim = GetComponent<Animator>();
            rb = parent.GetComponent<Rigidbody2D>();
            character = parent.GetComponent<PlayerMovementHandler>();
            climbData = parent.GetComponent<PlayerClimbingHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Set the Speed and ClimbSpeed parameters in the Animator component based on the character's velocity
        if(!character.onWall) anim.SetFloat("Speed", Mathf.Abs(character.rb.velocity.x));
        anim.SetFloat("ClimbSpeed", Mathf.Abs(character.rb.velocity.y));

        // Set the IsJumping parameter in
        // Set the IsJumping parameter in the Animator component based on whether the character is grounded
        anim.SetBool("IsJumping", !character.isGrounded);

        // Set the IsClimbing parameter in the Animator component based on whether the character is climbing
        anim.SetBool("IsClimbing", climbData.isClimbing);
    }
}