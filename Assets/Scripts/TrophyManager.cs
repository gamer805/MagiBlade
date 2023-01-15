using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrophyManager : MonoBehaviour
{
    public int trophyCount;
    public GameObject loader;
    public GameObject trophyText;
    public string loaderName;
    Collider2D bodyCollider;
    public GameObject[] trophies;
    public int requiredTrophies = 3;
    public AudioSource acquireTrophySound;

    void Start(){
        bodyCollider = GameObject.Find("Player").GetComponent<CharacterController>().bodyCollider;
    }

    // Update is called once per frame
    void Update()
    {
        trophies = GameObject.FindGameObjectsWithTag("Trophy");
        //init trophy text
        if (trophyText == null && GameObject.Find("Trophy Count") != null)
        {
            trophyText = GameObject.Find("Trophy Count");
        }

        //init win loader
        if (loader == null && GameObject.Find(loaderName) != null)
        {
            loader = GameObject.Find(loaderName);
        }

        //update trophy text
        if(trophyText != null)
            trophyText.GetComponent<TMP_Text>().text = "x" + trophyCount;

        //trigger win
        if(trophyCount >= requiredTrophies)
        {
            if(PlayerPrefs.GetFloat("bestTime") == null || PlayerPrefs.GetFloat("bestTime") == 0f || PlayerPrefs.GetFloat("currentTime") < PlayerPrefs.GetFloat("bestTime"))
                PlayerPrefs.SetFloat("bestTime", PlayerPrefs.GetFloat("currentTime"));
            Invoke("Load", 0.5f);
            
        }


        //detect trophy
        for (int i = 0; i < trophies.Length; i++)
        {
            if (bodyCollider.IsTouching(trophies[i].GetComponent<Collider2D>()) && trophies[i] != null)
            {
                Destroy(trophies[i]);
                trophyCount += 1;
                acquireTrophySound.Play();
            }
        }
        
    }

    void Load(){
        loader.GetComponent<SceneLoader>().Load();
        transform.position = new Vector3(0, 3.125f, 0);
        trophyCount = 0;
    }


}
