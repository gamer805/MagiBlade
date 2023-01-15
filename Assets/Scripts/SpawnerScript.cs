using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] prefabs;
    public bool Relay = false;
    public float SpawnFrequency = 0f;
    float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject obj = Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position, Quaternion.identity);
        obj.transform.parent = gameObject.transform;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= SpawnFrequency && Relay)
        {
            GameObject obj = Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position, Quaternion.identity);
            obj.transform.parent = gameObject.transform;
            timer = 0f;
        }

    }

}
