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

    void Start() {
        foreach(Transform entity in transform) {
            InitializeEntityRef(entity.gameObject);
        }
    }

    void Update() {
        

        if(roomCheckpoint.GetComponent<Checkpoint>().isActivated && !isActivated) {

            //StartCoroutine("ResetRoom");
            
            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
            foreach(GameObject room in rooms){
                if(room != gameObject) {
                    room.SetActive(false);
                }
            }

        }

        isActivated = roomCheckpoint.GetComponent<Checkpoint>().isActivated;

        if(PlayerDeathManager.dead && !isResetting && isActivated) {

            StartCoroutine("ResetRoom");

        } else if(!PlayerDeathManager.dead && isResetting && isActivated) {
            isResetting = false;
        }
    }

    void InitializeEntityRef(GameObject entity) {
        if(entity.GetComponent<AssetResetHandler>() != null) {
            string prefabRefName = entity.GetComponent<AssetResetHandler>().entityPrefab.name;
            GameObject prefabRef = prefabDictionary.Find(x => x.name == prefabRefName);
            if (entity.tag == "Enemy") {
                enemyLib.Add(entity);
                enemyPrefabLib.Add(prefabRef);
                enemyPositionLib.Add(entity.GetComponent<AssetResetHandler>().initCoords);
                enemyNameLib.Add(entity.name);
                shouldRespawnEnemy.Add(entity.GetComponent<DamageHandler>().respawnOnDeath);
            } else {
                entityLib.Add(entity);
                entityPrefabLib.Add(prefabRef);
                entityPositionLib.Add(entity.GetComponent<AssetResetHandler>().initCoords);
                entityNameLib.Add(entity.name);
            }
        }
    }

    IEnumerator ResetRoom() {
        yield return new WaitForSeconds(0.5f);
        isResetting = true;

        if(respawnDeadEnemies) {
            for(int i = 0; i < enemyLib.Count; i++) {
                if(enemyLib[i] == null && shouldRespawnEnemy[i]){
                    GameObject newObj = Instantiate(enemyPrefabLib[i], enemyPositionLib[i], Quaternion.identity);
                    newObj.transform.SetParent(transform);
                    newObj.name = enemyNameLib[i];
                    enemyLib[i] = newObj;
                } else if (enemyLib[i] != null && resetEnemyHealth) {
                    GameObject newObj = enemyLib[i].GetComponent<AssetResetHandler>().GetReplacement();
                    Destroy(enemyLib[i]);
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
                GameObject newObj = entityLib[i].GetComponent<AssetResetHandler>().GetReplacement();
                Destroy(entityLib[i]);
                entityLib[i] = newObj;
            }
        }
            
            
            
    }
}