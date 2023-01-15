using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    Animator anim;
    public GameObject contents;
    public Transform contentLoc;
    public bool open = false;
    public AudioSource openSound;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Open(){
        openSound.Play();
        anim.SetBool("open", true);
        Instantiate(contents, contentLoc.position, Quaternion.identity);
        open = true;

    }
}
