using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageHandler : MonoBehaviour
{
    public GameObject DropPrefab;
    public float health = 100f;
    public float maxHealth;

    public float flashTime = 0.1f;
    public int flashNum = 3;
    float alphaReduc = 0.5f;
    Color baseC;
    Color tempC;

    public bool knockedBack = false;
    public float knockValue = 3f;
    public Vector2 knockbackSpeed;

    public GameObject damageDealer;

    SpriteRenderer renderer;
    float timer = 0f;

    public ParticleSystem Blood;
    public AudioSource hitHurt;

    public GameObject sprite;
    public float zoomAmt = 0.2f;

    public bool canBeHurt = true;

    public float deathDelay = 0f;
    public Sprite deathSprite;

    public GameObject dependent;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        if(sprite == null || sprite.GetComponent<SpriteRenderer>() == null)
            renderer = GetComponent<SpriteRenderer>();
        else
            renderer = sprite.GetComponent<SpriteRenderer>();
        baseC = renderer.color;
        tempC = renderer.color;
        tempC.a = alphaReduc;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

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

    void cancelKnock()
    {
        if(gameObject.tag == "Player"){
                gameObject.layer = LayerMask.NameToLayer("Player");
                
            }
        knockedBack = false;
    }

    void cancelDamagePrevent(){
        canBeHurt = true;
    }

    public void applyKnock(int yKnock, float xKnockMod)
    {
        if(damageDealer != null){
            float moveDirection = (transform.position - damageDealer.transform.position).normalized.x;

            if(gameObject.tag == "Player"){
                gameObject.layer = LayerMask.NameToLayer("PlayerNoEnemies");
                
                GetComponent<PlayerMovementHandler>().rb.velocity = new Vector2(moveDirection * knockValue * knockbackSpeed.x, knockValue * knockbackSpeed.y);
            } else {
                GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection * knockValue * knockbackSpeed.x + xKnockMod * knockbackSpeed.x, knockValue * knockbackSpeed.y * yKnock);
            }
            
            Invoke("cancelKnock", 0.1f);
            
        }
    }
    public void applyKnock()
    {
        if(damageDealer != null){
            float moveDirection = (transform.position - damageDealer.transform.position).normalized.x;

            if(gameObject.tag == "Player"){
                GetComponent<PlayerMovementHandler>().rb.velocity = new Vector2(moveDirection * knockValue * knockbackSpeed.x, knockValue * knockbackSpeed.y);
            } else {
                GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection * knockValue * knockbackSpeed.x, knockValue * knockbackSpeed.y);
            }
            
            Invoke("cancelKnock", 0.4f);
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

    void AppDmgBase(float damage, GameObject enemy, float knockback, Vector2 knockAttributes)
    {
        if(canBeHurt && this.enabled){
            if (gameObject.name == "Player")
                canBeHurt = false;
                Invoke("cancelDamagePrevent", 1.5f);
            if(Blood != null && hitHurt != null) {CreateBlood(); hitHurt.Play();}
            timer = 0;
            damageDealer = enemy;
            health -= damage;
            knockedBack = true;
            knockValue = knockback;
            applyKnock((int) knockAttributes.x, knockAttributes.y);
            Camera.main.GetComponent<CameraZoom>().Zoom(zoomAmt);
            Camera.main.GetComponent<CameraZoom>().Reset();
        }
        
    }

    public void ApplyDamage(float damage, GameObject enemy, float knockback, Vector2 knockAttributes)
    {
        AppDmgBase(damage, enemy, knockback, knockAttributes);
        if(canBeHurt && this.enabled) StartCoroutine(colorFlash(flashNum, flashTime));
    }
    
    public void AppDmgNoColor(float damage, GameObject enemy, float knockback)
    {
        AppDmgBase(damage, enemy, knockback, Vector2.zero);
    }

    public void ApplyDamage(float damage, GameObject enemy, float knockback)
    {   
        AppDmgBase(damage, enemy, knockback, Vector2.zero);
        if(canBeHurt && this.enabled) StartCoroutine(colorFlash(flashNum, flashTime));
    }

    void Kill(){
        if(DropPrefab != null) {
            Instantiate(DropPrefab, transform.position, Quaternion.identity);
            Debug.Log("created");
        }
        
        if(dependent != null) dependent.GetComponent<Cobweb>().Disintigrate();

        if(gameObject.tag != "Player")
            Destroy(gameObject);
    }

    void CreateBlood(){
        Blood.Play();
    }
}
