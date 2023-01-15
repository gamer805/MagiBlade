using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{
    public bool hasKey = false;
    public Image UI;
    Collider2D bodyCollider;
    GameObject[] keys;
    GameObject[] chests;
    public Vector3 lastKeyLoc;
    public AudioSource acquireKeySound;

    void Start(){
        bodyCollider = GameObject.Find("Player").GetComponent<CharacterController>().bodyCollider;
    }

    void Update(){
        keys = GameObject.FindGameObjectsWithTag("key");
        chests = GameObject.FindGameObjectsWithTag("chest");

        foreach (GameObject key in keys)
        {
            if (bodyCollider.IsTouching(key.GetComponent<Collider2D>()) && !hasKey)
            {
                hasKey = true;
                acquireKeySound.Play();
                if(UI != null) UI.color = Color.white;
                lastKeyLoc = key.transform.position;
                Destroy(key);
            }
        }

        foreach (GameObject chest in chests)
        {
            if (bodyCollider.IsTouching(chest.GetComponent<Collider2D>()) && hasKey && Input.GetButtonDown("Fire2"))
            {
                ChestScript c = chest.GetComponent<ChestScript>();
                if(!c.open) {
                    c.Open();
                    hasKey = false;
                    if(UI != null) UI.color = Color.black;
                }
            }
        }
    }
}
