using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public DamageHandler damageManager;
    // Update is called once per frame
    void Update()
    {
       if(damageManager.enabled) transform.localScale = new Vector3( (damageManager.health/damageManager.maxHealth) * 10 , transform.localScale.y, transform.localScale.z);
    }
}
