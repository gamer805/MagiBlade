using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScript : MonoBehaviour
{
    public GameObject entityPrefab;
    Vector3 initCoords;

    void Start(){
        initCoords = transform.position;
    }

    public void Reset(){
        GameObject newObj = Instantiate(entityPrefab, initCoords, Quaternion.identity);
        newObj.transform.SetParent(transform.parent);
        newObj.name = gameObject.name;
        Debug.Log("Entity Reset");
        Destroy(gameObject);

    }
    
}
