using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbScript : MonoBehaviour
{
    float vertical;
    float jump;
    public float speed = 8f;
    public bool onLadder;
    public bool isClimbing;
    Rigidbody2D rb;
    public float gScale;

    public GameObject Weapon;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        if(onLadder){
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical*speed);
        } else {
            rb.gravityScale = gScale;
            isClimbing = false;
        }

        Weapon.SetActive(!isClimbing);
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "ladder"){
            onLadder = true;
            GetComponent<CharacterController>().rb.velocity = Vector2.zero;
        }
    }

    void OnTriggerStay2D(Collider2D col){
        if(col.gameObject.tag == "ladder"){
            onLadder = true;
            GetComponent<CharacterController>().rb.velocity = new Vector2(0, rb.velocity.y);
            Vector3 newPos = transform.position;
            newPos.x = col.gameObject.transform.position.x;
            transform.position = newPos;
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(col.CompareTag("ladder")){
            onLadder = false;
        }
    }
}
