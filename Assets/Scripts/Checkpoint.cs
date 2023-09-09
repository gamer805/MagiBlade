using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    //reference to CameraManager script component
    public CameraManager camScript;
    //reference to collider component
    public BoxCollider2D col;
    //handles activation
    public bool checkpointCamActive = false;
    //reference to corresponding room
    public GameObject room;

    void Start(){
        if(room.GetComponent<RoomResetHandler>() != null){
            room.GetComponent<RoomResetHandler>().checkpoint = gameObject;
        }
    }

    void Update(){
        if(camScript.enabled && !checkpointCamActive){
            room.SetActive(true);
        }
        checkpointCamActive = camScript.enabled;
    }

    //draws collider size in editor
    public void OnDrawGizmosSelected(){
        //get collider component
        col = GetComponent<BoxCollider2D>();
        //set collider size to be slightly larger than CameraManager view
        col.size = new Vector2(camScript.width + (camScript.maxPos.x - camScript.minPos.x) - 1.5f, camScript.height + (camScript.maxPos.y - camScript.minPos.y));
        //set collider offset to be center of CameraManager view
        col.offset = new Vector2( (camScript.minPos.x+camScript.maxPos.x)/2 - transform.position.x, (camScript.minPos.y+camScript.maxPos.y)/2 - transform.position.y);
    }

    //switches to this CameraManager script
    public void switchCam(){
        //disable all other CameraManager scripts
        foreach(CameraManager cam in Camera.main.gameObject.GetComponents<CameraManager>()){
            cam.enabled = false;
        }
        //enable this CameraManager script
        camScript.enabled = true;
    }
}
