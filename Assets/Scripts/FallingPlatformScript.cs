using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformScript : MonoBehaviour
{

    Rigidbody2D rbody;
    Animator anim;
    public float gScale = 2f;
    Vector3 initLoc;
    bool inContact = false;
    public GameObject selfPrefab;
    public float contactTime = 0.2f;
    public AudioSource CrumbleAudio;
    public AudioSource ReinstateAudio;
    bool fell = false;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rbody.isKinematic = true;
        initLoc = transform.position;
        gameObject.layer = 18;
    }

    void Update()
    {
        if(transform.position.y < initLoc.y-10f)
        {
            Invoke("Reinstate", 1f);
        }
        if(rbody.isKinematic){
            rbody.velocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            inContact = true;
            Invoke("AnimateFall", contactTime);
        }
            
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            inContact = false;
        }

    }

    void AnimateFall()
    {
        if(inContact && !fell)
            anim.SetTrigger("Shake");
    }

    public void Fall()
    {
        fell = true;
        rbody.gravityScale = gScale;
        rbody.isKinematic = false;
        CrumbleAudio.Play();
        inContact = false;
        gameObject.layer = 19;
    }

    void Reinstate()
    {
        GameObject clone = Instantiate(selfPrefab, initLoc, Quaternion.identity);
        clone.GetComponent<Animator>().SetTrigger("Initialize");
        clone.GetComponent<FallingPlatformScript>().ReinstateAudio.playOnAwake = true;
        clone.transform.parent = transform.parent;
         Destroy(gameObject);
        
    }
}
