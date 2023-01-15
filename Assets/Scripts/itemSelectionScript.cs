using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemSelectionScript : MonoBehaviour
{
    public Transform child;
    public GameObject overlay;
    public itemBarScript itemBar;
    public int childID = 0;
    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0);
        overlay = transform.GetChild(0).GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount > 0)
            child = transform.GetChild(childID);
        overlay.transform.SetParent(child);
        overlay.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        if (Input.GetKeyUp(KeyCode.X))
        {
            childID++;
            if (childID > transform.childCount - 1)
            {
                childID = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            childID--;
            if (childID < 0)
            {
                childID = transform.childCount - 1;
            }
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (childID == transform.childCount - 1)
            {
                childID--;
            }
            child.transform.SetParent(itemBar.openItemSpace);
            child.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        
    }

    public void AddItem(int id){
        if(transform.childCount > 0)
            child = transform.GetChild(id);

        if (id == transform.childCount - 1)
        {
            id--;
        }
        child.transform.SetParent(itemBar.openItemSpace);
        child.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        itemBar.openItemID++;
        itemBar.DetectOpenSpace();
    }
}
