using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpHurtDetector : MonoBehaviour
{
    public float dmg = 10f;
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "PlayerFoot"){
            GameObject.Find("Player").GetComponent<CharacterController>().Jump();
            gameObject.transform.parent.gameObject.GetComponent<Damagable>().ApplyDamage(dmg, col.gameObject, 1f, new Vector2(0, 1.5f));
        }
    }
}
