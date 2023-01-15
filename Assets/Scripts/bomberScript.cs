using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomberScript : MonoBehaviour
{
    GameObject player;

    public float range;
    public float thrust;

    public GameObject bomb;
    public Transform projPos;

    float timer = 0;
    public float resetTime = 2f;

    Animator anim;

    bool facingRight = false;

    void Start()
    {
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(Vector2.Distance(player.transform.position, transform.position) <= range){
            if(timer > resetTime){
                timer = 0;
                Throw();
            }
        }

        if(facingRight && transform.position.x > player.transform.position.x) Flip();
        if(!facingRight && transform.position.x < player.transform.position.x) Flip();
    }

    void Throw()
    {
        GameObject proj = Instantiate(bomb, projPos.position, Quaternion.identity);
        anim.SetTrigger("Throw");
        Vector2 dir = (player.transform.position-transform.position).normalized;
        proj.GetComponent<Rigidbody2D>().AddForce(dir*thrust, ForceMode2D.Impulse);


    }

    void Flip()
    {
        if (facingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingRight = false;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingRight = true;
        }
    }
}
