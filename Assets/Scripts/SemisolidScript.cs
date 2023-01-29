using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemisolidScript : MonoBehaviour
{
    public bool onPlayer = false;
    GameObject player;
    public AudioSource WoodThunk;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

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
