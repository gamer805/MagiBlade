using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{
    public bool attackLimit = false;
    public int attackLimitNum = 5;

    public float knockback = 3f;

    public Transform attackPoint;
    public float attackDamage = 20f;
    public float attackRate = 2f;
    public float attackTime = 0.1f;

    Animator meleeAnim;
    public GameObject meleeWeapon;

    public float cooldownTime = 0f;

    EnemyMovementHandler npc;
    GameObject target;
    public bool attacking = false;
    public bool animateAttack = false;

    void Start()
    {
        npc = GetComponent<EnemyMovementHandler>();
        if (meleeWeapon != null)
            meleeAnim = meleeWeapon.GetComponent<Animator>();
    }

    void Update()
    {
        cooldownTime += Time.deltaTime;
        if(npc.currentTarget != null)
            target = npc.currentTarget;

        if (npc.engaged && npc.inRange)
        {
            if (cooldownTime >= attackRate)
            {
                cooldownTime = 0;
                StartCoroutine(Attack());
            }

        }
        
        if(attackLimitNum <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Attack()
    {
        attacking = true;
        if(meleeWeapon != null)
            meleeAnim.SetTrigger("Swing");
        yield return new WaitForSeconds(attackTime);
        if (npc.inRange && target != null)
        {
            npc.attackCooldown = true;
            DealDamage();
            yield return new WaitForSeconds(attackTime);
            npc.attackCooldown = false;
        }
    }
    void DealDamage()
    {
        target.GetComponent<DamageHandler>().ApplyDamage(attackDamage, gameObject, knockback);
        if (attackLimit)
            attackLimitNum--;

    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Player"){
            cooldownTime = 0;
            StartCoroutine(Attack());
        }
    }
}
