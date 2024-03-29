﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenuParameters : MonoBehaviour
{
    Vector3 ogPos;
    Transform par;
    public int id;
    public GameObject prefab;
    public string type = "melee";
    public Text descriptionText;
    public string name;
    public string addedDescription;
    public Transform overlay;

    void Start()
    {
        ogPos = transform.position;
        par = transform.parent;
    }

    void Update()
    {
        if(overlay.parent == gameObject.transform)
        {
            ActivateDescription();
        }
    }

    public void Clear()
    {
        transform.position = ogPos;
        transform.parent = par;
    }

    public void ActivateDescription()
    {
        descriptionText.text = name + "\n\n\n";
        if (type == "melee")
        {
            descriptionText.fontSize = 25;

            descriptionText.text += "Attack: " + prefab.GetComponent<MeleeWeapon>().damage
                + " hp\n\n" + "Reload: " + prefab.GetComponent<MeleeWeapon>().reloadTime
                + " s\n\n" + "Knockback: " + prefab.GetComponent<MeleeWeapon>().knockbackPower + " m";
        }
        else if (type == "projectile")
        {
            descriptionText.fontSize = 20;

            descriptionText.text += "Attack: " + prefab.GetComponent<Projectile>().damage
                + " hp\n\n" + "Reload: " + prefab.GetComponent<Projectile>().reloadTime
                + " s\n\n" + "Knockback: " + prefab.GetComponent<Projectile>().knockbackPower + " m";
        }
        else if (type == "bow")
        {
            descriptionText.fontSize = 20;
            Projectile projectileStats = prefab.transform.GetChild(0).gameObject.GetComponent<Projectile>();
            descriptionText.text += "Attack: " + projectileStats.damage
                + " hp\n\n" + "Reload: " + projectileStats.reloadTime
                + " s\n\n" + "Knockback: " + projectileStats.knockbackPower + " m";
        }
        else if (type == "troop")
        {
            GameObject troop = prefab.GetComponent<Projectile>().resultPrefab;
            EnemyAttackHandler attackStats = troop.GetComponent<EnemyAttackHandler>();
            EnemyMovementHandler mobilityStats = troop.GetComponent<EnemyMovementHandler>();
            DamageHandler healthStats = troop.GetComponent<DamageHandler>();

            descriptionText.fontSize = 15;

            descriptionText.text += "Troop Attack: " + attackStats.attackDamage + "hp"
                + "\n\nTroop Health: " + healthStats.health + "hp"
                + "\n\nTroop Speed: " + mobilityStats.walkSpeed + " m"
                + "\n\nTroop Attack Rate: " + attackStats.attackRate + " s"
                + "\n\nTroop Knockback: " + attackStats.knockbackPower + " m";
        }

        if(addedDescription != "")
        {
            descriptionText.text += "\n\n\n" + addedDescription;
        }
    }
}
