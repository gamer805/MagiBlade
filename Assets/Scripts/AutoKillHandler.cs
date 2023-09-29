using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKillHandler : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            StartCoroutine(other.gameObject.GetComponent<PlayerDeathManager>().KillPlayer());
        }
    }
}
