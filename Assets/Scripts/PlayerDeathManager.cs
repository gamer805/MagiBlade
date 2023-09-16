using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeathManager : MonoBehaviour
{
    DamageHandler damager;
    SpriteRenderer renderer;
    PlayerMovementHandler controller;
    Rigidbody2D rb;

    public GameObject healthDisplay;
    public GameObject weaponContainer;
    public Transform checkpoint;

    public GameObject keyPrefab;
    public Key keyData;

    int healthPercentage = 100;
    public float respawnDelay = 0.3f;
    
    public static bool dead = false;
    
    
    
    public AudioSource deathSound;
    public Animator screenFadeAnimator;

    void Start()
    {
        damager = GetComponent<DamageHandler>();
        renderer = damager.sprite.GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerMovementHandler>();
        rb = GetComponent<Rigidbody2D>();
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

            if(keyData.hasKey){
                keyData.hasKey = false;
                Instantiate(keyPrefab, keyData.lastKeyLoc, Quaternion.identity);
                keyData.UI.color = Color.black;
            }

            dead = true;
            deathSound.Play();
            screenFadeAnimator.SetTrigger("Fade");
            damager.health = 0;
            damager.enabled = false;
            renderer.enabled = false;
            controller.enabled = false;
            rb.bodyType = RigidbodyType2D.Static;
            weaponContainer.SetActive(false);
            yield return new WaitForSeconds(respawnDelay);
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
        
        if(damager.damageDealer != null && damager.damageDealer.GetComponent<EnemyMovementHandler>() != null){
            damager.damageDealer.GetComponent<EnemyMovementHandler>().inRange = false;
            damager.damageDealer.GetComponent<EnemyMovementHandler>().inSight = false;
        }
        
        controller.enabled = true;
        controller.rb.velocity = Vector2.zero;
        rb.velocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Dynamic;
        renderer.enabled = true;
        weaponContainer.SetActive(true);
    }

    void OnTriggerStay2D(Collider2D col){
        if(col.gameObject.tag == "checkpoint" && checkpoint != col.gameObject.transform){
            checkpoint = col.gameObject.transform;
            checkpoint.GetComponent<Checkpoint>().switchCam();
        }
    }
}
