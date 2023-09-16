using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DependencyHandler))]
public class Blockade : MonoBehaviour
{
    Animator Anim;
    BoxCollider2D collider;

    DependencyHandler dependencyHandler;
    GameObject cameraRef;

    bool opened = false;

    // Start is called before the first frame update
    void Start()
    {
        cameraRef = Camera.main.gameObject;
        Anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        dependencyHandler = GetComponent<DependencyHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!opened && !dependencyHandler.dependencyExists){
            opened = true;
            OpenDoor();
        }
    }

    public void OpenDoor(){
        Anim.SetBool("open", true);
        collider.enabled = false;
        cameraRef.GetComponent<CameraScreenShake>().Shake(0.5f, 0.25f);
    }

    public void CloseDoor(){
        Anim.SetBool("open", false);
        collider.enabled = true;
    }
}
