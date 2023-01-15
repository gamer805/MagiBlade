﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class spellEffector : MonoBehaviour
{
    SpriteRenderer renderer;
    npcMoveScript moveStats;
    npcAttackScript attackStats;
    public GameObject parent;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        if(parent == null){
            moveStats = GetComponent<npcMoveScript>();
            attackStats = GetComponent<npcAttackScript>();
        } else {
            moveStats = parent.GetComponent<npcMoveScript>();
            attackStats = parent.GetComponent<npcAttackScript>();
        }
       
    }

    public IEnumerator courFreeze(Color color, float duration, float speedRedux)
    {
        float initSpeed = moveStats.initSpeed;
        renderer.color = color;
        moveStats.speed = initSpeed * speedRedux;
        yield return new WaitForSeconds(duration);
        renderer.color = Color.white;
        moveStats.speed = initSpeed;
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