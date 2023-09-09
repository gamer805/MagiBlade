using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeathManager : MonoBehaviour
{
    public GameObject healthDisplay;
    int healthPercentage = 100;
    public Transform checkpoint;
    public float deathTime = 0.3f;
    GameObject weaponHolder;
    public static bool dead = false;
    public Key keyData;
    public GameObject keyPref;
    DamageHandler damager;
    SpriteRenderer renderer;
    PlayerMovementHandler controller;
    Rigidbody2D rb;
    public ParticleSystem Blood;
    public AudioSource hitHurt;
    public AudioSource deathSound;
    public Animator fade;

    void Start()
    {
        damager = GetComponent<DamageHandler>();
        renderer = damager.sprite.GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerMovementHandler>();
        rb = GetComponent<Rigidbody2D>();
        weaponHolder = transform.GetChild(0).gameObject;
        healthPercentage = Mathf.RoundToInt(GetComponent<DamageHandler>().health / 500) * 100;
        if (healthDisplay == null)
        {

            healthDisplay = GameObject.Find("Player Health");
        }

    }
    // Update is called once per frame
    void Update()
    {
        
        healthPercentage = Mathf.RoundToInt(gameObject.GetComponent<DamageHandler>().health / 5);
        if(healthPercentage <= 0) healthPercentage = 0;
        if(healthDisplay != null)
            healthDisplay.GetComponent<UnityEngine.UI.Text>().text = "Health: " + healthPercentage + "%";
        if (healthDisplay == null)
        {

            healthDisplay = GameObject.Find("Player Health");
        }
        if (gameObject.GetComponent<DamageHandler>().health <= 0)
        {
            StartCoroutine(KillPlayer());
            
        }
            
    }

    public IEnumerator KillPlayer(){
        if(!dead){
            DamageHandler damager = GetComponent<DamageHandler>();
            SpriteRenderer renderer = damager.sprite.GetComponent<SpriteRenderer>();
            PlayerMovementHandler controller = GetComponent<PlayerMovementHandler>();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if(Blood != null && hitHurt != null) {CreateBlood(); hitHurt.Play();}

            if(keyData.hasKey){
                keyData.hasKey = false;
                Instantiate(keyPref, keyData.lastKeyLoc, Quaternion.identity);
                keyData.UI.color = Color.black;
            }

            dead = true;
            deathSound.Play();
            fade.SetTrigger("Fade");
            damager.health = 0;
            damager.enabled = false;
            renderer.enabled = false;
            controller.enabled = false;
            rb.bodyType = RigidbodyType2D.Static;
            weaponHolder.SetActive(false);
            npcMoveScript[] npcs = FindObjectsOfType(typeof(npcMoveScript)) as npcMoveScript[];
            foreach(npcMoveScript npc in npcs){
                npc.targets.Clear();
                npc.currentTarget = null;
            }

            yield return new WaitForSeconds(deathTime);
            Respawn(damager, renderer, controller, rb);
        }
        
        
    }

    public void Respawn(DamageHandler damager, SpriteRenderer renderer, PlayerMovementHandler controller, Rigidbody2D rb){
        transform.position = checkpoint.position;
        controller.movingRight = true;
        transform.eulerAngles = new Vector3(0, 0, 0);
        dead = false;
        damager.enabled = true;
        damager.health = 500f;
        
        if(damager.damageDealer != null && damager.damageDealer.GetComponent<npcMoveScript>() != null){
            damager.damageDealer.GetComponent<npcMoveScript>().inRange = false;
            damager.damageDealer.GetComponent<npcMoveScript>().engaged = false;
        }
        
        controller.enabled = true;
        controller.rb.velocity = Vector2.zero;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Dynamic;
        renderer.enabled = true;
        weaponHolder.SetActive(true);
    }

    void OnTriggerStay2D(Collider2D col){
        if(col.gameObject.tag == "checkpoint" && checkpoint != col.gameObject.transform){
            checkpoint = col.gameObject.transform;
            checkpoint.GetComponent<Checkpoint>().switchCam();
        }
    }

    void CreateBlood(){
        Blood.Play();
    }
}
