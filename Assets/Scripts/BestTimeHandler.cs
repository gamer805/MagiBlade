using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BestTimeManager : MonoBehaviour
{
    float timer;
    string timeStr;
    public Leaderboard leaderboard;

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetFloat("bestTime") != null && PlayerPrefs.GetFloat("bestTime") != 0){
            timer = PlayerPrefs.GetFloat("bestTime");
            timeStr = "Best Time - " + Mathf.Floor(timer/60f) + ":";
            if(Mathf.Floor(timer)%60f < 10){
                timeStr += "0" + Mathf.Floor(timer)%60f;
            } else {
                timeStr += Mathf.Floor(timer)%60f;
            }
        } else {
            if(leaderboard.GatherScoreByID() != -1){
                PlayerPrefs.SetFloat("bestTime", leaderboard.GatherScoreByID());
            }
            
            timeStr = "Speedrun";
        }
        
        
        gameObject.GetComponent<TMP_Text>().text = timeStr;
    }
}
