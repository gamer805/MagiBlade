using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDamageHandler : MonoBehaviour
{
    public float dmg = 10f;
    public bool contacting = false;
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "PlayerFoot"){
            contacting = true;
            GameObject.Find("Player").GetComponent<PlayerMovementHandler>().Jump();
            gameObject.transform.parent.gameObject.GetComponent<DamageHandler>().ApplyDamage(dmg, col.gameObject, 1f, new Vector2(0, 1.5f));
        }
    }
}
