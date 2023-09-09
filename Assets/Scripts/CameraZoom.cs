using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera CameraManager;
    float goalSize;
    float startSize;
    public float zoomSpeed;
    bool resetting = false;
    // Start is called before the first frame update
    void Start()
    {
        CameraManager = Camera.main;
        goalSize = CameraManager.orthographicSize;
        startSize = CameraManager.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(goalSize - CameraManager.orthographicSize) > 0.05){
            CameraManager.orthographicSize = Mathf.MoveTowards(CameraManager.orthographicSize, goalSize, zoomSpeed*Time.deltaTime);
        } else if (resetting){
            resetting = false;
            goalSize = startSize;
        }
    }

    public void Zoom(float scale){
        goalSize -= scale;
    }

    public void Reset(){
        resetting = true;
    }
}
