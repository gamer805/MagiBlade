using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float timer = 2f;
    public GameObject explosionPrefab;
    public float attackDamage = 20f;
    public float knockback = 3f;
    public bool isEnemyProjectile = true;
    public float thrust = 10f;
    public bool impactDissipation = true;
    GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (isEnemyProjectile)
        {
            if (col.gameObject.tag == "Player")
                col.gameObject.GetComponent<DamageHandler>().ApplyDamage(attackDamage, gameObject, knockback);
        }
        if(impactDissipation)
            Detonate();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            Detonate();
        }
    }

    void Detonate(){
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
