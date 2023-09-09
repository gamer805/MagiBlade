using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetResetHandler : MonoBehaviour
{
    public GameObject entityPrefab;
    [HideInInspector] public Vector3 initCoords;

    void Start(){
        initCoords = transform.position;
        if(transform.parent.tag == "Room" && transform.tag == "Enemy"){
            foreach(GameObject enemy in transform.parent.GetComponent<RoomResetHandler>().enemyPrefabDict){
                if(enemy.name == entityPrefab.name){
                    entityPrefab = enemy;
                    break;
                }
            }
        }
    }

    public GameObject Reset(){
        GameObject newObj = Instantiate(entityPrefab, initCoords, Quaternion.identity);
        newObj.transform.SetParent(transform.parent);
        newObj.name = gameObject.name;
        if(GetComponent<DamageHandler>().dependent != null)
            newObj.GetComponent<DamageHandler>().dependent = GetComponent<DamageHandler>().dependent;
        return newObj;

    }
    
}
