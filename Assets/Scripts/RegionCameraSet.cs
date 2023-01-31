using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionCameraSet : MonoBehaviour
{
    //reference to camera script component
    public camera camScript;
    //reference to collider component
    public BoxCollider2D col;

    public bool checkpointCamActive = false;

    void Update(){
        checkpointCamActive = camScript.enabled;
    }

    //draws collider size in editor
    public void OnDrawGizmosSelected(){
        //get collider component
        col = GetComponent<BoxCollider2D>();
        //set collider size to be slightly larger than camera view
        col.size = new Vector2(camScript.width + (camScript.maxPos.x - camScript.minPos.x) - 1.5f, camScript.height + (camScript.maxPos.y - camScript.minPos.y));
        //set collider offset to be center of camera view
        col.offset = new Vector2( (camScript.minPos.x+camScript.maxPos.x)/2 - transform.position.x, (camScript.minPos.y+camScript.maxPos.y)/2 - transform.position.y);
    }

    //switches to this camera script
    public void switchCam(){
        //disable all other camera scripts
        foreach(camera cam in Camera.main.gameObject.GetComponents<camera>()){
            cam.enabled = false;
        }
        //enable this camera script
        camScript.enabled = true;
    }
}
