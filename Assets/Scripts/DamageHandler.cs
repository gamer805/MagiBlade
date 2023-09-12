using System.Threading;
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

    float alphaReduc = 0.5f;
    Color baseC;
    Color tempC;

    [HideInInspector] public GameObject damageDealer;

    Rigidbody2D rb;
    SpriteRenderer renderer;
    float invulnerabilityTimer = 0f;
    public float invulnerabilityLength = 0.5f;

    public ParticleSystem Blood;
    public AudioSource hitAudio;

    public GameObject sprite;
    public float zoomAmount = 0.2f;

    bool canBeDamaged = true;

    public float deathDelay = 0f;
    public Sprite deathSprite;

    public GameObject dependentAsset;
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
        baseC = renderer.color;
        tempC = renderer.color;
        tempC.a = alphaReduc;
    }

    // Update is called once per frame
    void Update()
    {
        invulnerabilityTimer += Time.deltaTime;

        if (invulnerabilityTimer < invulnerabilityLength) {
            canBeDamaged = false;
        } else {
            canBeDamaged = true;
        }

        if (health <= 0)
        {
            if(gameObject.tag != "Player"){
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

    void cancelKnockback()
    {
        if(gameObject.tag == "Player"){
                gameObject.layer = LayerMask.NameToLayer("Player");
                
            }
        knockedBack = false;
    }

    public void applyKnockback(float xKnockMod = 0, float yKnockMod = 1)
    {
        if(damageDealer != null){
            float moveDirection = (transform.position - damageDealer.transform.position).normalized.x;

            if(gameObject.tag == "Player"){
                gameObject.layer = LayerMask.NameToLayer("Invulnerable");
                rb.velocity = new Vector2(moveDirection * baseKnockback * knockbackSpeed.x, baseKnockback * knockbackSpeed.y);
            } else {
                rb.velocity = new Vector2(moveDirection * baseKnockback * knockbackSpeed.x + xKnockMod * knockbackSpeed.x, baseKnockback * knockbackSpeed.y * yKnockMod);
            }
            
            Invoke("cancelKnockback", 0.1f);
            
        }
    }

    IEnumerator colorFlash(int num, float time)
    {
        for (int i = 0; i < num; i++)
        {
            renderer.color = tempC;
            yield return new WaitForSeconds(time);
            renderer.color = baseC;
            yield return new WaitForSeconds(time);
        }
    }

    public void ApplyDamage(float damage, GameObject enemy, float knockbackPower, Vector2? knockbackSpeed = null, bool applyFlash = true)
    {
        if(canBeDamaged && this.enabled){
            if (gameObject.name == "Player")
                canBeDamaged = false;
                invulnerabilityTimer = 0;
                
            if(Blood != null && hitAudio != null) {
                Blood.Play();
                hitAudio.Play();
            }
            damageDealer = enemy;
            health -= damage;

            
            knockedBack = true;
            baseKnockback = knockbackPower;
            Vector2 effectiveKnockbackSpeed = knockbackSpeed ?? new Vector2(0, 1);
            applyKnockback(effectiveKnockbackSpeed.x, effectiveKnockbackSpeed.y);
            
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
            dependentAsset.GetComponent<Cobweb>().Disintigrate();
        }

        if(gameObject.tag != "Player") {
            Destroy(gameObject);
        }
    }
}
