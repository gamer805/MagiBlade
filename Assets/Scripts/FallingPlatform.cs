using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{

    Rigidbody2D rbody;
    public Animator anim;
    
    public GameObject selfPrefab;
    public AudioSource CrumbleAudio;
    public AudioSource ReinstateAudio;

    public string baseLayer;
    public string fallingLayer;

    public float gScale = 2f;
    public float contactTime = 0.2f;
    public float resetDelay = 0.5f;

    Vector3 initLoc;
    bool inContact = false;
    bool fell = false;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.isKinematic = true;
        initLoc = transform.position;
        gameObject.layer = LayerMask.NameToLayer(baseLayer);
    }

    void Update()
    {
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
        anim.enabled = false;
        rbody.gravityScale = gScale;
        rbody.isKinematic = false;
        CrumbleAudio.Play();
        inContact = false;
        gameObject.layer = LayerMask.NameToLayer(fallingLayer);
        StartCoroutine("Reinstate");
    }

    IEnumerator Reinstate()
    {
        yield return new WaitForSeconds(resetDelay);
        GameObject clone = Instantiate(selfPrefab, initLoc, Quaternion.identity);
        clone.GetComponent<FallingPlatform>().anim.enabled = true;
        clone.GetComponent<FallingPlatform>().anim.SetTrigger("Initialize");
        clone.GetComponent<FallingPlatform>().ReinstateAudio.playOnAwake = true;
        clone.transform.parent = transform.parent;
        clone.name = transform.name;
        Destroy(gameObject);
    }
}
