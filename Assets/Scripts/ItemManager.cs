using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemDict;
    public string[] itemNames;
    public GameObject currentItem;
    public string itemName = "nothing";
    public Sprite defaultSprite;
    Sprite lastSprite;
    public Image displayImg;
    float reloadTime;

    
    void Start()
    {
        SetItem(itemName);
        displayImg.sprite = defaultSprite;
        lastSprite = defaultSprite;

    }

    void Update(){
        if(displayImg != null && defaultSprite != lastSprite){
            displayImg.sprite = defaultSprite;
            lastSprite = defaultSprite;
        }
    }

    public void SetItem(string name)
    {
        itemName = name;

        if(transform.childCount > 0){
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        if(itemName != "nothing" && itemName != null){
            currentItem = Instantiate(itemDict[Array.IndexOf(itemNames, name)], transform.position, transform.parent.rotation);
            currentItem.transform.SetParent(transform);
        }
        


    }
}
