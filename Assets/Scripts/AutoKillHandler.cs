using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKillHandler : MonoBehaviour
{
    public Collider2D playerCol;

    void Start(){
        playerCol = GameObject.Find("Player").GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Player" && col == playerCol){
            StartCoroutine(col.gameObject.GetComponent<PlayerDeathManager>().KillPlayer());
        }
    }
}
