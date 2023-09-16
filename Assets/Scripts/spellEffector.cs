using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class spellEffector : MonoBehaviour
{
    SpriteRenderer renderer;
    EnemyMovementHandler moveStats;
    EnemyAttackHandler attackStats;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        moveStats = GetComponent<EnemyMovementHandler>();
        attackStats = GetComponent<EnemyAttackHandler>();
       
    }

    public IEnumerator courFreeze(Color color, float duration, float speedRedux)
    {
        float initialSpeed = moveStats.initialSpeed;
        renderer.color = color;
        moveStats.walkSpeed = initialSpeed * speedRedux;
        yield return new WaitForSeconds(duration);
        renderer.color = Color.white;
        moveStats.walkSpeed = initialSpeed;
    }
    public IEnumerator courWeaken(Color color, float duration, float redux)
    {
        float initAttack = attackStats.attackDamage;
        renderer.color = color;
        attackStats.attackDamage = initAttack * redux;
        yield return new WaitForSeconds(duration);
        renderer.color = Color.white;
        attackStats.attackDamage = initAttack;
    }

    public void Freeze(Color color, float duration, float speedRedux)
    {
        StartCoroutine(courFreeze(color, duration, speedRedux));
    }
    public void Weaken(Color color, float duration, float speedRedux)
    {
        StartCoroutine(courWeaken(color, duration, speedRedux));
    }
}
