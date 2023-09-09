using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject[] activeItems;
    public GameObject[] containers;
    public GameObject itemBar;
    public bool isFull = false;
    public GameObject overlay;
    public GameObject[] itemPrefabs;
    public GameObject weaponHolder;
    // Start is called before the first frame update
    void Start()
    {
        activeItems = new GameObject[4];
        itemPrefabs = new GameObject[4];
    }

    // Update is called once per frame
    void Update()
    {
        isFull = Full();
        if (!isFull)
        {
            for (int i = 0; i < containers.Length; i++)
            {
                if(containers[i].transform.childCount > 0)
                {
                    activeItems[i] = containers[i].transform.GetChild(0).gameObject;
                    itemPrefabs[i] = activeItems[i].GetComponent<ItemMenuParameters>().prefab;
                }
            }
        } else
        {
            overlay.SetActive(false);
        }
        
    }

    bool Full()
    {
        int counter = 0;
        for (int i = 0; i < activeItems.Length; i++)
        {
            if(activeItems[i] != null)
            {
                counter++;
            }
        }
        if (counter == 4)
            return true;
        else
            return false;
    }

    public void Clear()
    {
        if (isFull)
        {
            foreach (GameObject item in activeItems)
            {
                item.GetComponent<ItemMenuParameters>().Clear();
                item.transform.SetSiblingIndex(item.GetComponent<ItemMenuParameters>().id);
            }
            GetComponent<ItemHotBar>().openItemID = 0;
            itemBar.GetComponent<itemSelectionScript>().childID = 0;
            activeItems = new GameObject[4];
            overlay.SetActive(true);
            overlay.transform.parent = itemBar.transform.GetChild(0);
        }
    }

    public void Confirm()
    {
        if (isFull)
        {
            for (int i = 0; i < activeItems.Length; i++)
            {
                PlayerPrefs.SetInt(""+(i+1), activeItems[i].GetComponent<ItemMenuParameters>().id);
            }
        }
    }

    public void Fill(){
        for (int i = 0; i < 4; i++)
        {
            itemBar.GetComponent<itemSelectionScript>().AddItem( PlayerPrefs.GetInt(""+(i+1)) );
        }
        
    }
}
