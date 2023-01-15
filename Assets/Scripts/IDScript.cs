using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IDScript : MonoBehaviour
{
    void Start(){
        Invoke("DisplayText", 1.0f);
    }
    void DisplayText()
    {
        gameObject.GetComponent<TMP_Text>().text = "ID - #" + PlayerPrefs.GetInt("memberID");
        if(PlayerPrefs.GetString("NGID") != null && PlayerPrefs.GetString("NGID") != "") {
            gameObject.GetComponent<TMP_Text>().text = "ID - " + PlayerPrefs.GetString("NGID");
        }
    }
}
