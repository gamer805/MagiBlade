using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomResetScript : MonoBehaviour
{
    bool hasReset = false;
    public GameObject checkpoint;
    public bool activated;
    bool activeOnLastFrame = false;

    public bool respawnDeadEnemies = false;
    public List<GameObject> enemyPrefabDict;
    public List<GameObject> enemyList;
    public List<GameObject> enemyBackups;
    public List<Vector3> enemyPosBackups;
    public List<string> enemyNameBackups;

    void Start(){
        if(respawnDeadEnemies){
            foreach(Transform enemy in transform){
                if(enemy.tag == "Enemy" && enemy.GetComponent<ResetScript>() != null){
                    enemyList.Add(enemy.gameObject);

                    GameObject enemyPrefab = enemy.GetComponent<ResetScript>().entityPrefab;
                    foreach(GameObject prefab in enemyPrefabDict){
                        if(prefab.name == enemyPrefab.name.Split(new char[] { ' ' })[0]){
                            enemyBackups.Add(prefab);
                        }
                    }
                    
                    enemyPosBackups.Add(enemy.GetComponent<ResetScript>().initCoords);
                    enemyNameBackups.Add(enemy.name);
                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        activated = checkpoint.GetComponent<CheckpointScript>().checkpointCamActive;

        if(activated && !activeOnLastFrame){
            if(respawnDeadEnemies) ReplaceDeadEnemies();
            
            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
            foreach(GameObject room in rooms){
                if(room != gameObject){
                    room.SetActive(false);
                }
            }
        }

        if(PlayerDeathManager.dead && !hasReset && activated){

            if(respawnDeadEnemies) ReplaceDeadEnemies();
            hasReset = true;

            List<Transform> roomEntities = new List<Transform>();

            foreach(Transform entity in transform){
                roomEntities.Add(entity);
            }

            foreach(Transform entity in roomEntities){

                if(entity.GetComponent<ResetScript>() != null){
                    StartCoroutine(DelayedReset(entity.gameObject));
                }
            }

        } else if(!PlayerDeathManager.dead && hasReset && activated){
            hasReset = false;
        }

        activeOnLastFrame = activated;

    }

    void ReplaceDeadEnemies(){
        foreach(GameObject enemy in enemyList){
            if(enemy == null){
                Debug.Log("Empty Spot Found");
                GameObject entityPrefab = enemyBackups[enemyList.IndexOf(enemy)];
                Vector3 initCoords = enemyPosBackups[enemyList.IndexOf(enemy)];
                GameObject newObj = Instantiate(entityPrefab, initCoords, Quaternion.identity);
                newObj.transform.SetParent(transform);
                newObj.name = enemyNameBackups[enemyList.IndexOf(enemy)];
                enemyList[enemyList.IndexOf(enemy)] = newObj;
            }
        }
    }

    IEnumerator DelayedReset(GameObject entity){
        yield return new WaitForSeconds(0.5f);
        Debug.Log("attempted delayed reset");
        int listID = 0;
        if(entity != null){
            if(respawnDeadEnemies) {
                listID = enemyList.IndexOf(entity);
            }

            GameObject newEntity = entity.GetComponent<ResetScript>().Reset();
            Destroy(entity);

            if(respawnDeadEnemies && listID >= 0) {
                enemyList[listID] = newEntity;
            }
            Debug.Log(newEntity.name + " Reset");
        }
            
            
            
    }
}
