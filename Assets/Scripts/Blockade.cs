using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockade : MonoBehaviour
{
    Animator Anim;
    BoxCollider2D collider;

    public GameObject dependantAsset;
    GameObject cam;

    bool opened = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.gameObject;
        Anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!opened && dependantAsset == null){
            OpenDoor();
        }
    }

    public void OpenDoor(){
        opened = true;
        Anim.SetBool("open", true);
        collider.enabled = false;
        cam.GetComponent<CameraScreenShake>().Shake(0.5f, 0.25f);
    }

    public void CloseDoor(){
        opened = false;
        Anim.SetBool("open", false);
        collider.enabled = true;
    }
}
