using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LootLocker.Requests;
using UnityEngine;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    
    public int memberID;
    public int playerScore;
    public int ID;
    public int maxScores = 3;
    public TMP_Text[] Entries;
    bool done = false;
    int registeredScore = -1;
    bool gatherscore = false;
    bool sessionStarted = false;

    void Start(){
        float unroundedScore = PlayerPrefs.GetFloat("bestTime");
        playerScore = (int) Mathf.Floor(unroundedScore);
        if(PlayerPrefs.GetInt("memberID") == null || PlayerPrefs.GetInt("memberID") == 0){
            PlayerPrefs.SetInt("memberID", Random.Range(0,99999999));
        }
        memberID = PlayerPrefs.GetInt("memberID");

        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session; " + response.Error);
                
                return;
            }

            Debug.Log("successfully started LootLocker session");
            sessionStarted = true;
            StartCoroutine(ScoreRoutine());
        });

        

        
    }

    void Update(){
        if(gatherscore && sessionStarted){
            LootLockerSDKManager.GetMemberRank(ID, FindID(), (response) =>
            {
                if(response.success){
                    registeredScore = response.score;
                    gatherscore = false;
                } else {
                    gatherscore = false;
                }
            });
        }
        
    }

    public int GatherScoreByID(){
        gatherscore = true;
        return registeredScore;
    }

    IEnumerator ScoreRoutine(){
        
        LootLockerSDKManager.GetMemberRank(ID, FindID(), (response) =>
        {
            if(response.success){
                if(response.score != playerScore){
                    Debug.Log("Score Submit");
                    SubmitScore();
                }
            } else {
                SubmitScore();
            }
        });
        yield return new WaitForSeconds(2f);
        LootLockerSDKManager.GetScoreList(ID, maxScores, (response) =>
        {
            if(response.success){
                
                LootLockerLeaderboardMember[] scores = response.items;
                for(int i = 0; i < scores.Length; i++){
                    Debug.Log("Score: " + scores[i].score);
                    int truescore = scores[i].score;
                    string timeStr = Mathf.Floor(truescore/60f) + ":";
                    if(Mathf.Floor(truescore)%60f < 10){
                        timeStr += "0" + Mathf.Floor(truescore)%60f;
                    } else {
                        timeStr += Mathf.Floor(truescore)%60f;
                    }
                    Entries[i].text = (scores[i].rank + ".    " + timeStr + " - #" + scores[i].member_id);
                }

                if(scores.Length < maxScores){
                    for(int i = scores.Length; i < maxScores; i++){
                        Entries[i].text = (i+1).ToString()  + ".    " + "None";
                    }
                }

            } else {
                Debug.Log("Failed");
            }
        });

    }
    public string FindID(){
        if(PlayerPrefs.GetString("NGID") == null || PlayerPrefs.GetString("NGID") == ""){
            return memberID.ToString();
        }else{
            return PlayerPrefs.GetString("NGID");
        }
    }

    public void SubmitScore(){
            LootLockerSDKManager.SubmitScore(FindID(), playerScore, ID, (response) =>
            {
                if(response.success){
                    Debug.Log("Success! " + FindID() + ", " + playerScore);
                } else {
                    Debug.Log("Failed");
                }
            });
        
    }

}
