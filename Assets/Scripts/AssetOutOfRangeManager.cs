using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetOutOfRangeManager : MonoBehaviour
{
    GameObject player;
    bool moveEnabled;
    RigidbodyType2D rbtype;
    bool isOn = true;
    public bool roomMode = false;
    bool roomActivated = true;
    bool lastRoomState = true;
    RoomResetScript roomResetter;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if(roomMode) roomResetter = transform.parent.GetComponent<RoomResetScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(roomMode) roomActivated = roomResetter.activated;
        float dist = Vector3.Distance(player.transform.position, transform.position);

        if(dist > 40f && isOn && !roomMode){
            DisableObj();
        } else if (dist <= 40f && !isOn && !roomMode){
            EnableObj();
        }

        if(roomMode && !roomActivated && lastRoomState){
            DisableObj();
        } else if (roomMode && roomActivated && !lastRoomState){
            EnableObj();
        }

        lastRoomState = roomActivated;
        
    }

    void DisableObj(){
        isOn = false;

        rbtype = GetComponent<Rigidbody2D>().bodyType;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        Component[] components = gameObject.GetComponents(typeof(Behaviour));
        foreach(Behaviour component in components) {
            if(component != GetComponent<Rigidbody2D>() &&
            component != GetComponent<AssetOutOfRangeManager>() &&
            component != gameObject.transform) {
                component.enabled = false;
            }
        }
        
    }

    void EnableObj(){
        isOn = true;
        GetComponent<Rigidbody2D>().bodyType = rbtype;

        Component[] components = gameObject.GetComponents(typeof(Behaviour));
        foreach(Behaviour component in components) {
            if(component != GetComponent<Rigidbody2D>() &&
            component != GetComponent<AssetOutOfRangeManager>() &&
            component != gameObject.transform) {
                component.enabled = true;
            }
        }
    }

}
