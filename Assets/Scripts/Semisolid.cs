using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semisolid : MonoBehaviour
{
    public bool onPlayer = false;
    public AudioSource WoodThunk;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") < 0 && onPlayer) {
            gameObject.layer = LayerMask.NameToLayer("Semisolid");
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            WoodThunk.Play();
            onPlayer = true;
        } else if (col.gameObject.tag == "PlayerFoot") {
            gameObject.layer = LayerMask.NameToLayer("Platform");
        }
            
    }

    void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            onPlayer = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player") {
            onPlayer = false;
        }

    }

}
