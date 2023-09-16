using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MovementType {
    Enemy,
    Player
}
public class SquashStretchHandler : MonoBehaviour
{
    bool wasGrounded = false;
    bool isGrounded;
    public Animator heightAnimator;

    public MovementType movementType;

    void FixedUpdate()
    {

        if (movementType == MovementType.Player) {
            isGrounded = GetComponent<PlayerMovementHandler>().isGrounded;
        } else if (movementType == MovementType.Enemy) {
            isGrounded = GetComponent<EnemyMovementHandler>().isGrounded;
        }

        if (isGrounded && !wasGrounded) {
            heightAnimator.SetTrigger("squash");
        } else if (!isGrounded && wasGrounded) {
            heightAnimator.SetTrigger("stretch");
        }

        wasGrounded = isGrounded;


    }
}
