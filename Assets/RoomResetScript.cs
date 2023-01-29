using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomResetScript : MonoBehaviour
{
    bool hasReset = false;
    // Update is called once per frame
    void Update()
    {
        if(PlayerDeathManager.dead && !hasReset){
            hasReset = true;
            List<Transform> roomEntities = new List<Transform>();
            foreach(Transform entity in transform){
                roomEntities.Add(entity);
            }
            foreach(Transform entity in roomEntities){

                if(entity.GetComponent<ResetScript>() != null){
                    StartCoroutine(DelayedReset(entity));
                }
                
            }
        } else if(!PlayerDeathManager.dead && hasReset){
            hasReset = false;
        }
    }

    IEnumerator DelayedReset(Transform entity){
        yield return new WaitForSeconds(0.5f);
        entity.GetComponent<ResetScript>().Reset();
    }
}
