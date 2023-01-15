using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera camera;
    float goalSize;
    float startSize;
    public float zoomSpeed;
    bool resetting = false;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        goalSize = camera.orthographicSize;
        startSize = camera.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(goalSize - camera.orthographicSize) > 0.05){
            camera.orthographicSize = Mathf.MoveTowards(camera.orthographicSize, goalSize, zoomSpeed*Time.deltaTime);
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
