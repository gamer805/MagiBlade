using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetOutOfRangeManager : MonoBehaviour
{
    GameObject player;
    bool moveEnabled;
    RigidbodyType2D rbtype;
    bool isOn = true;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if(dist > 25f && isOn){
            isOn = false;
            if(GetComponent<npcMoveScript>() != null)
                moveEnabled = GetComponent<npcMoveScript>().enabled == true;
            rbtype = GetComponent<Rigidbody2D>().bodyType;
            if(GetComponent<npcMoveScript>() != null)
                GetComponent<npcMoveScript>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            foreach(MonoBehaviour comp in GetComponents<MonoBehaviour>()){
                if(comp != gameObject.GetComponent<AssetOutOfRangeManager>() && comp.enabled == true) comp.enabled = false;
            }
        } else if(dist < 25f && !isOn){
            isOn = true;
            if(GetComponent<npcMoveScript>() != null)
                GetComponent<npcMoveScript>().enabled = moveEnabled;
            GetComponent<Rigidbody2D>().bodyType = rbtype;
            foreach(MonoBehaviour comp in GetComponents<MonoBehaviour>()){
                if(comp != gameObject.GetComponent<AssetOutOfRangeManager>() && comp.enabled == false) comp.enabled = true;
            }
        }
    }


}
