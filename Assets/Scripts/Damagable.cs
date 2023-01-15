using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damagable : MonoBehaviour
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
            if(DropPrefab != null) {
                Instantiate(DropPrefab, transform.position, Quaternion.identity);
                Debug.Log("created");
            }
            if(gameObject.tag != "Player")
                Destroy(gameObject);
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
                
                GetComponent<CharacterController>().rb.velocity = new Vector2(moveDirection * knockValue * knockbackSpeed.x, knockValue * knockbackSpeed.y);
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
                GetComponent<CharacterController>().rb.velocity = new Vector2(moveDirection * knockValue * knockbackSpeed.x, knockValue * knockbackSpeed.y);
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

    public void ApplyDamage(float damage, GameObject enemy, float knockback, Vector2 knockAttributes)
    {
        if(canBeHurt)
        {
            Debug.Log(enemy.name + " hit " + gameObject.name + " at " + Time.time + ".");
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
            StartCoroutine(colorFlash(flashNum, flashTime));
        }
    }
    public void ApplyDamage(float damage, GameObject enemy, float knockback)
    {   
        if(canBeHurt)
        {   
            Debug.Log(enemy.name + " hit " + gameObject.name + " at " + Time.time + ".");
            if (gameObject.name == "Player")
                canBeHurt = false;
                Invoke("cancelDamagePrevent", 1.5f);
            if(Blood != null && hitHurt != null) {CreateBlood(); hitHurt.Play();}
            timer = 0;
            damageDealer = enemy;
            health -= damage;
            knockedBack = true;
            knockValue = knockback;
            applyKnock();
            Camera.main.GetComponent<CameraZoom>().Zoom(zoomAmt);
            Camera.main.GetComponent<CameraZoom>().Reset();
            StartCoroutine(colorFlash(flashNum, flashTime));
        }
    }
    public void AppDmgNoColor(float damage, GameObject enemy, float knockback)
    {
        if(canBeHurt){
            Debug.Log(enemy.name + " hit " + gameObject.name + " at " + Time.time + ".");
            if (gameObject.name == "Player")
                canBeHurt = false;
                Invoke("cancelDamagePrevent", 1.5f);
            if(Blood != null && hitHurt != null) {CreateBlood(); hitHurt.Play();}
            timer = 0;
            damageDealer = enemy;
            health -= damage;
            knockedBack = true;
            knockValue = knockback;
            applyKnock();
        }
        
    }
    void CreateBlood(){
        Blood.Play();
    }
}
