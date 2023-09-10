using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomResetHandler : MonoBehaviour
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
            CollectEnemyRefs();
        }
    }

    void Update()
    {
        activated = checkpoint.GetComponent<Checkpoint>().checkpointCamActive;

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
            Reset();
        } else if(!PlayerDeathManager.dead && hasReset && activated){
            hasReset = false;
        }

        activeOnLastFrame = activated;

    }

    void CollectEnemyRefs(){
        foreach(Transform enemy in transform){
            if(enemy.tag == "Enemy" && enemy.GetComponent<AssetResetHandler>() != null) {
                enemyList.Add(enemy.gameObject);

                GameObject selfPrefab = enemy.GetComponent<AssetResetHandler>().entityPrefab;
                string prefabName = selfPrefab.name.Split(new char[] { ' ' })[0];
                IEnumerable<GameObject> rawEnemyBackups = from prefab in enemyPrefabDict where prefab.name == prefabName select prefab;
                enemyBackups = rawEnemyBackups.ToList();
                
                enemyPosBackups.Add(enemy.GetComponent<AssetResetHandler>().initCoords);
                enemyNameBackups.Add(enemy.name);
            }
        }
    }
    void ReplaceDeadEnemies(){
        foreach(GameObject enemy in enemyList){
            if(enemy == null){
                GameObject entityPrefab = enemyBackups[enemyList.IndexOf(enemy)];
                Vector3 initCoords = enemyPosBackups[enemyList.IndexOf(enemy)];
                GameObject newObj = Instantiate(entityPrefab, initCoords, Quaternion.identity);
                newObj.transform.SetParent(transform);
                newObj.name = enemyNameBackups[enemyList.IndexOf(enemy)];
                enemyList[enemyList.IndexOf(enemy)] = newObj;
            }
        }
    }

    void Reset() {
        if(respawnDeadEnemies) ReplaceDeadEnemies();
        hasReset = true;
        IEnumerable<Transform> roomEntities = from Transform entity in transform where entity.GetComponent<AssetResetHandler>() != null select entity;

        foreach(Transform entity in roomEntities) {
            StartCoroutine(DelayedReset(entity.gameObject));
        }
    }

    IEnumerator DelayedReset(GameObject entity) {
        yield return new WaitForSeconds(0.5f);
        int listID = 0;
        if(entity != null){
            if(respawnDeadEnemies) {
                listID = enemyList.IndexOf(entity);
            }

            GameObject newEntity = entity.GetComponent<AssetResetHandler>().Reset();
            Destroy(entity);

            if(respawnDeadEnemies && listID >= 0) {
                enemyList[listID] = newEntity;
            }
        }   
    }
}
