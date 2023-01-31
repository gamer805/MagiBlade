using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScript : MonoBehaviour
{
    public GameObject entityPrefab;
    [HideInInspector] public Vector3 initCoords;

    void Start(){
        initCoords = transform.position;
    }

    public GameObject Reset(){
        GameObject newObj = Instantiate(entityPrefab, initCoords, Quaternion.identity);
        newObj.transform.SetParent(transform.parent);
        newObj.name = gameObject.name;
        return newObj;

    }
    
}
