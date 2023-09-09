using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
[ExecuteInEditMode]
public class CameraTextureHandler : MonoBehaviour
{
    public RenderTexture targetTex;
    Camera cam;
    // Start is called before the first frame update
    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.targetTexture = targetTex;
    }
}
