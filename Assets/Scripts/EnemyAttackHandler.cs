using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHandler : MonoBehaviour
{

    public float knockbackPower = 3f;

    public Transform attackPoint;
    public float attackDamage = 20f;
    public float attackRate = 2f;
    public float attackTime = 0.1f;
    public bool isCoolingDown = false;

    Animator meleeAnim;
    public GameObject meleeWeapon;

    public float cooldownTime = 0f;

    EnemyMovementHandler movementData;
    GameObject playerRef;
    public bool isAttacking = false;
    public bool animateAttack = false;

    void Start()
    {
        movementData = GetComponent<EnemyMovementHandler>();
        playerRef = GameObject.Find("Player");
        if (meleeWeapon != null) {
            meleeAnim = meleeWeapon.GetComponent<Animator>();
        }
    }

    void Update()
    {
        cooldownTime += Time.deltaTime;
        if (playerRef == null) {
            playerRef = GameObject.Find("Player");
        }

        if (movementData.inSight && movementData.inRange && cooldownTime >= attackRate) {
            cooldownTime = 0;
            StartCoroutine(Attack());

        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        if(meleeWeapon != null) {
            meleeAnim.SetTrigger("Swing");
        }
        yield return new WaitForSeconds(attackTime);
        if (movementData.inRange && playerRef != null) {
            isCoolingDown = true;
            playerRef.GetComponent<DamageHandler>().ApplyDamage(attackDamage, gameObject, knockbackPower);
            yield return new WaitForSeconds(attackTime);
            isCoolingDown = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Player"){
            cooldownTime = 0;
            StartCoroutine(Attack());
        }
    }
}
