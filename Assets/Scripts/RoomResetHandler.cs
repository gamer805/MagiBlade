using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomResetHandler : MonoBehaviour
{
    bool isResetting = false;
    public GameObject roomCheckpoint;
    public bool isActivated;

    public bool respawnDeadEnemies = false;
    public bool resetEnemyHealth = false;

    public List<GameObject> prefabDictionary;
    public List<GameObject> enemyLib;

    public List<GameObject> enemyPrefabLib;
    public List<Vector3> enemyPositionLib;
    public List<string> enemyNameLib;
    public List<bool> shouldRespawnEnemy;

    public List<GameObject> entityLib;

    public List<GameObject> entityPrefabLib;
    public List<Vector3> entityPositionLib;
    public List<string> entityNameLib;

    public int refID = 0;

    void Start() {
        foreach(Transform entity in transform) {
            if(entity.GetComponent<AssetResetHandler>() != null) {
            string prefabRefName = entity.GetComponent<AssetResetHandler>().entityPrefabName;
            GameObject prefabRef = prefabDictionary.Find(x => x.name == prefabRefName);
            if (entity.tag == "Enemy") {
                enemyLib.Add(entity.gameObject);
                enemyPrefabLib.Add(prefabRef);
                enemyPositionLib.Add(entity.GetComponent<AssetResetHandler>().initCoords);
                enemyNameLib.Add(entity.name);
                shouldRespawnEnemy.Add(entity.GetComponent<DamageHandler>().respawnOnDeath ? true : false);
            } else {
                entityLib.Add(entity.gameObject);
                entityPrefabLib.Add(prefabRef);
                entityPositionLib.Add(entity.GetComponent<AssetResetHandler>().initCoords);
                entityNameLib.Add(entity.name);
            }
            }
        }
        foreach(Checkpoint checkpoint in FindObjectsOfType(typeof(Checkpoint))) {
            if (checkpoint.refID == refID) {
                roomCheckpoint = checkpoint.gameObject;
            }
        }
    }

    void Update() {
        

        if(roomCheckpoint.GetComponent<Checkpoint>().isActivated && !isActivated) {

            ResetRoom();
            
            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
            foreach(GameObject room in rooms){
                if(room != gameObject) {
                    room.SetActive(false);
                }
            }

        }

        isActivated = roomCheckpoint.GetComponent<Checkpoint>().isActivated;

        if(PlayerDeathManager.dead && !isResetting && isActivated) {

            Invoke("ResetRoom", 0.5f);

        } else if(!PlayerDeathManager.dead && isResetting && isActivated) {
            isResetting = false;
        }
    }

    void ResetRoom() {
        isResetting = true;

        if(respawnDeadEnemies) {
            for(int i = 0; i < enemyLib.Count; i++) {
                if(enemyLib[i] == null && shouldRespawnEnemy[i]){
                    GameObject newObj = Instantiate(enemyPrefabLib[i], enemyPositionLib[i], Quaternion.identity);
                    newObj.transform.SetParent(transform);
                    newObj.name = enemyNameLib[i];
                    enemyLib[i] = newObj;
                    
                } else if (enemyLib[i] != null && resetEnemyHealth) {
                    Destroy(enemyLib[i]);
                    GameObject newObj = Instantiate(enemyPrefabLib[i], enemyPositionLib[i], Quaternion.identity);
                    newObj.transform.SetParent(transform);
                    newObj.name = enemyNameLib[i];
                    enemyLib[i] = newObj;
                }
            }
        }

        for(int i = 0; i < entityLib.Count; i++) {
            if(entityLib[i] == null) {
                GameObject newObj = Instantiate(entityPrefabLib[i], entityPositionLib[i], Quaternion.identity);
                newObj.transform.SetParent(transform);
                newObj.name = entityNameLib[i];
                entityLib[i] = newObj;
            } else {
                Destroy(entityLib[i]);
                GameObject newObj = Instantiate(enemyPrefabLib[i], enemyPositionLib[i], Quaternion.identity);
                newObj.transform.SetParent(transform);
                newObj.name = enemyNameLib[i];
                entityLib[i] = newObj;
            }
        }
            
            
            
    }
}