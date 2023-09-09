
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedrunTimer : MonoBehaviour
{
    public float inittime;
    float timer;
    string timeStr;
    // Start is called before the first frame update
    void Start()
    {
        inittime = Time.time;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer = Time.time - inittime;
        timeStr = "Time: " + Mathf.Floor(timer/60f) + ":";
        if(Mathf.Floor(timer)%60f < 10){
            timeStr += "0" + Mathf.Floor(timer)%60f;
        } else {
            timeStr += Mathf.Floor(timer)%60f;
        }
        gameObject.GetComponent<TMP_Text>().text = timeStr;
        PlayerPrefs.SetFloat("currentTime", timer);
    }
}
