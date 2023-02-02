using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScript : MonoBehaviour
{
    public GameObject entityPrefab;
    [HideInInspector] public Vector3 initCoords;

    void Start(){
        initCoords = transform.position;
        if(transform.parent.tag == "Room" && transform.tag == "Enemy"){
            foreach(GameObject enemy in transform.parent.GetComponent<RoomResetScript>().enemyPrefabDict){
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
        if(GetComponent<Damagable>().dependent != null)
            newObj.GetComponent<Damagable>().dependent = GetComponent<Damagable>().dependent;
        return newObj;

    }
    
}
