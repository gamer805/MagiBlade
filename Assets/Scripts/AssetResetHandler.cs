using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetResetHandler : MonoBehaviour
{
    public string entityPrefabName;
    [HideInInspector] public Vector3 initCoords;

    void Start(){
        initCoords = transform.position;
    }

    public GameObject GetReplacement(GameObject entityPrefab){
        GameObject newObj = Instantiate(entityPrefab, initCoords, Quaternion.identity);
        newObj.transform.SetParent(transform.parent);
        newObj.name = gameObject.name;
        if(GetComponent<DamageHandler>() != null && GetComponent<DamageHandler>().dependentAsset != null) {
            newObj.GetComponent<DamageHandler>().dependentAsset = GetComponent<DamageHandler>().dependentAsset;
        }
        return newObj;

    }
    
}
