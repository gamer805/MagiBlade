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
        if (col.gameObject.tag == "Player"){
            WoodThunk.Play();
            onPlayer = true;
        }
            
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            onPlayer = false;
        }
            

    }
    void ReinstateLayer()
    {
        gameObject.layer = 18;
        
    }
}
