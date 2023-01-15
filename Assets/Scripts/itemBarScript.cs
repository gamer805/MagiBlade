using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBarScript : MonoBehaviour
{
    public Transform openItemSpace;
    public int openItemID = 0;
    public InventoryManager inventory;
    // Start is called before the first frame update
    void Start()
    {
        inventory = GetComponent<InventoryManager>();
        DetectOpenSpace();
        inventory.Fill();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (openItemID < 4 && openItemSpace.childCount > 0)
        {
            openItemID++;
        }
        if(openItemID > transform.childCount - 1)
        {
            openItemID = 0;
        }
        DetectOpenSpace();
    }

    public void DetectOpenSpace(){
        if (transform.GetChild(openItemID) != null)
            openItemSpace = transform.GetChild(openItemID);
    }
}
