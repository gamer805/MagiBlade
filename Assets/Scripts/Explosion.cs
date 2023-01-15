using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float attackDamage = 25f;
    public float knockback = 3f;
    public bool isEnemyExplosive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isEnemyExplosive)
        {
            if (col.gameObject.tag == "Player")
                col.gameObject.GetComponent<Damagable>().ApplyDamage(attackDamage, gameObject, knockback);
        }
    }

    public void Dissipate()
    {
        Destroy(gameObject);
    }

}
