using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemisolidScript : MonoBehaviour
{
    public bool onPlayer = false;
    public AudioSource WoodThunk;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") < 0 && onPlayer)
        {
            gameObject.layer = 19;
        }
        if (Input.GetAxis("Jump") > 0 ||
        Input.GetAxis("Vertical") > 0 ||
        PlayerDeathManager.dead)
        {
            ReinstateLayer();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "PlayerFoot"){
            WoodThunk.Play();
            onPlayer = true;
        } else if (col.gameObject.tag == "Player") {
            foreach(Collider2D wallCol in col.gameObject.GetComponent<WallCollider>().WallCols)
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), wallCol);
        }
            
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "PlayerFoot")
        {
            onPlayer = false;
        }
            

    }
    void ReinstateLayer()
    {
        gameObject.layer = 18;
        
    }
}
