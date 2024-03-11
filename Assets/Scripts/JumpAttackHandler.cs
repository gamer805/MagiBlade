using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackHandler : MonoBehaviour
{
    public LayerMask targetLayers; // Layers to target for raycast
    public float jumpDamage = 10f; // Determines damage done by jump attack
    public float jumpPower = 5f; // Determines knockback power from jump attack

    public PlayerMovementHandler playerMovementHandler;

    void Update()
    {
        // Check if raycast is touching target layer
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 1f, targetLayers);

        // Loop through all hits
        foreach (RaycastHit2D hit in hits)
        {
            DamageHandler damageHandler = hit.collider.GetComponent<DamageHandler>();
            if (damageHandler != null)
            {
                playerMovementHandler.Jump();
                playerMovementHandler.canDoubleJump = true;

                // Apply damage to DamageHandler
                damageHandler.ApplyDamage(jumpDamage, gameObject, jumpPower);
            }
        }
    }
}
