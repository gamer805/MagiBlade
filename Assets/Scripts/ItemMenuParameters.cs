using System.Collections;
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

            descriptionText.text += "Attack: " + prefab.GetComponent<Melee>().damage
                + " hp\n\n" + "Reload: " + prefab.GetComponent<Melee>().reloadTime
                + " s\n\n" + "Knockback: " + prefab.GetComponent<Melee>().knockback + " m";
        }
        else if (type == "projectile")
        {
            descriptionText.fontSize = 20;

            descriptionText.text += "Attack: " + prefab.GetComponent<Projectile>().damage
                + " hp\n\n" + "Reload: " + prefab.GetComponent<Projectile>().reloadTime
                + " s\n\n" + "Knockback: " + prefab.GetComponent<Projectile>().knockback + " m";
        }
        else if (type == "bow")
        {
            descriptionText.fontSize = 20;
            Projectile projectileStats = prefab.transform.GetChild(0).gameObject.GetComponent<Projectile>();
            descriptionText.text += "Attack: " + projectileStats.damage
                + " hp\n\n" + "Reload: " + projectileStats.reloadTime
                + " s\n\n" + "Knockback: " + projectileStats.knockback + " m";
        }
        else if (type == "troop")
        {
            GameObject troop = prefab.GetComponent<Projectile>().resultPrefab;
            npcAttackScript attackStats = troop.GetComponent<npcAttackScript>();
            npcMoveScript mobilityStats = troop.GetComponent<npcMoveScript>();
            Damagable healthStats = troop.GetComponent<Damagable>();

            descriptionText.fontSize = 15;

            descriptionText.text += "Troop Attack: " + attackStats.attackDamage + "hp"
                + "\n\nTroop Health: " + healthStats.health + "hp"
                + "\n\nTroop Speed: " + mobilityStats.speed + " m"
                + "\n\nTroop Attack Rate: " + attackStats.attackRate + " s"
                + "\n\nTroop Knockback: " + attackStats.knockback + " m";
        }

        if(addedDescription != "")
        {
            descriptionText.text += "\n\n\n" + addedDescription;
        }
    }
}
