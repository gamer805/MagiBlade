using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageHandler : MonoBehaviour
{
    public GameObject DropPrefab;
    public float health = 100f;
    [HideInInspector] public float maxHealth;

    public float flashTime = 0.1f;
    int flashNum = 3;

    [HideInInspector] public bool knockedBack = false;
    public float baseKnockback = 3f;
    public Vector2 knockbackSpeed;
    Vector2 knockbackModifier;
    bool knockbackApplied = false;

    float transparencyModifier = 0.5f;
    Color baseColor;
    Color damagedColor;

    [HideInInspector] public GameObject damageDealer;

    Rigidbody2D rb;
    SpriteRenderer renderer;
    float invulnerabilityTimer = 0f;
    public float invulnerabilityLength = 0.5f;
    bool isInvulnerable = true;

    public ParticleSystem Blood;
    public AudioSource hitAudio;

    public GameObject sprite;
    public float zoomAmount = 0.2f;


    public float deathDelay = 0f;
    public Sprite deathSprite;

    public GameObject dependentAsset;

    public enum EntityType {
        Enemy,
        Player
    }
    public EntityType entityType;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        invulnerabilityTimer = invulnerabilityLength;
        if(sprite == null || sprite.GetComponent<SpriteRenderer>() == null)
            renderer = GetComponent<SpriteRenderer>();
        else
            renderer = sprite.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        baseColor = renderer.color;
        damagedColor = renderer.color;
        damagedColor.a = transparencyModifier;
    }

    void Update() {

        invulnerabilityTimer += Time.deltaTime;

        isInvulnerable = (invulnerabilityTimer < invulnerabilityLength);

        if (health <= 0) {
            if(entityType == EntityType.Enemy) {
                Component[] components = gameObject.GetComponents(typeof(Behaviour));
                foreach(Behaviour component in components) {
                    if(component != GetComponent<Rigidbody2D>() &&
                    component != GetComponent<Collider2D>() &&
                    component != GetComponent<SpriteRenderer>() &&
                    component != gameObject.transform) {
                        component.enabled = false;
                    }
                }
                if(deathSprite != null) {
                    GetComponent<SpriteRenderer>().sprite = deathSprite;
                }
            }
            
            Invoke("Kill", deathDelay);
        }
    }

    void FixedUpdate() {
        if (knockedBack) {
            if(damageDealer != null){
                if (entityType == EntityType.Player) {
                    GetComponent<PlayerMovementHandler>().enabled = true;
                } else {
                    GetComponent<EnemyMovementHandler>().enabled = true;
                }
                float moveDirection = (transform.position - damageDealer.transform.position).normalized.x;
                rb.velocity = new Vector2(moveDirection * baseKnockback * knockbackSpeed.x + knockbackModifier.x * knockbackSpeed.x, baseKnockback * knockbackModifier.y * knockbackSpeed.y);

                StartCoroutine(cancelKnockback());
            }
        }
    }

    IEnumerator cancelKnockback() {
        yield return new WaitForSeconds(0.1f);
        knockedBack = false;
    }

    IEnumerator colorFlash(int num, float time)
    {
        for (int i = 0; i < num; i++)
        {
            renderer.color = damagedColor;
            yield return new WaitForSeconds(time);
            renderer.color = baseColor;
            yield return new WaitForSeconds(time);
        }
    }

    public void ApplyDamage(float damage, GameObject enemy, float knockbackPower, Vector2? knockbackSpeed = null, bool applyFlash = true)
    {
        if(!isInvulnerable && this.enabled){

            isInvulnerable = true;
            invulnerabilityTimer = 0;
                
            if(Blood != null && hitAudio != null) {
                Blood.Play();
                hitAudio.Play();
            }
            damageDealer = enemy;
            health -= damage;

            
            knockedBack = true;
            baseKnockback = knockbackPower;
            knockbackModifier = knockbackSpeed ?? Vector2.up;

            if (entityType == EntityType.Player) {
                GetComponent<PlayerMovementHandler>().enabled = false;
            } else {
                GetComponent<EnemyMovementHandler>().enabled = false;
            }
            
            Camera.main.GetComponent<CameraZoom>().Zoom(zoomAmount);
            Camera.main.GetComponent<CameraZoom>().Reset();

            if(applyFlash) StartCoroutine(colorFlash(flashNum, flashTime));
        }
        
    }

    void Kill(){
        if(DropPrefab != null) {
            Instantiate(DropPrefab, transform.position, Quaternion.identity);
        }
        
        if(dependentAsset != null) {
            dependentAsset.GetComponent<DependencyHandler>().UseBehavior();
        }

        if(entityType == EntityType.Enemy) {
            Destroy(gameObject);
        }
    }
}
