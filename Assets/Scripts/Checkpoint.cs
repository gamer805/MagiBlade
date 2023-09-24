using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CameraManager cameraManager;
    public int cameraID = 0;
    public BoxCollider2D col;
    public bool isActivated = false;
    public GameObject room;

    void Start(){
        foreach(CameraManager manager in Camera.main.gameObject.GetComponents<CameraManager>()){
            if (manager.cameraID == cameraID) {
                cameraManager = manager;
            }
        }
        if(room.GetComponent<RoomResetHandler>() != null){
            room.GetComponent<RoomResetHandler>().roomCheckpoint = gameObject;
        }
    }

    void Update(){
        if(cameraManager.enabled && !isActivated){
            room.SetActive(true);
        }
        isActivated = cameraManager.enabled;
    }

    public void OnDrawGizmosSelected(){
        col = GetComponent<BoxCollider2D>();
        col.size = new Vector2(cameraManager.width + (cameraManager.maxPos.x - cameraManager.minPos.x) - 1.5f, cameraManager.height + (cameraManager.maxPos.y - cameraManager.minPos.y));
        col.offset = new Vector2( (cameraManager.minPos.x+cameraManager.maxPos.x)/2 - transform.position.x, (cameraManager.minPos.y+cameraManager.maxPos.y)/2 - transform.position.y);
    }

    public void switchCameraManager(){
        foreach(CameraManager manager in Camera.main.gameObject.GetComponents<CameraManager>()){
            manager.enabled = false;
        }
        cameraManager.enabled = true;
    }
}
