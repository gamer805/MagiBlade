using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKillHandler : MonoBehaviour
{
    Collider2D playerCol;


    void Start(){
        playerCol = GameObject.Find("Player").GetComponent<BoxCollider2D>();
    }

    void Update() {
        if (GetComponent<Collider2D>().IsTouching(playerCol)) {
            StartCoroutine(GameObject.Find("Player").GetComponent<PlayerDeathManager>().KillPlayer());
        }
    }
}
