using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semisolid : MonoBehaviour
{
    public bool onPlayer = false;
    bool canReinstate = false;
    public AudioSource WoodThunk;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") < 0 && onPlayer) {
            gameObject.layer = LayerMask.NameToLayer("Semisolid");
        }
        if (canReinstate) {
            canReinstate = false;
            gameObject.layer = LayerMask.NameToLayer("Platform");
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            WoodThunk.Play();
            onPlayer = true;
        } else if (col.gameObject.tag == "PlayerFoot") {
            canReinstate = true;
        }
            
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player") {
            onPlayer = false;
        }

    }

}
