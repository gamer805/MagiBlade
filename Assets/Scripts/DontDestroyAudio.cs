using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyAudio : MonoBehaviour
{
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        

        DontDestroyOnLoad(this.gameObject);
    }

    void Update(){
         GameObject mainThemeSupport = GameObject.FindWithTag("mainThemeSupport");
        if(mainThemeSupport == null) Destroy(gameObject);
    }
}
