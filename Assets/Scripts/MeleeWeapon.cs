using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public float damage;
    public float knockback;
    public string source;
    public string enemy;
    public float reloadTime;
    public Color reloadCol;
    public Color defaultCol;
    float timer;
    public bool extended = false;
    public float extensionTime;
    public Vector3 offset;

    Animator anim;
    SpriteRenderer renderer;
    public AudioSource attackSound;

    // Start is called before the first frame update
    void Start()
    {
        timer = reloadTime;
        transform.localPosition = offset;
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer < reloadTime)
        {
            renderer.color = reloadCol;
        } else
        {
            renderer.color = defaultCol;
        }

        if (Input.GetButtonDown("Fire1") && timer >= reloadTime && !extended && gameObject.layer != 23)
        {
            extended = true;
            timer = 0;
            anim.SetTrigger("Used");
            Invoke("Rescend", extensionTime);
            if(attackSound != null) attackSound.Play();
        }
        
    }

    void Rescend()
    {
        extended = false;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == enemy && extended)
        {
            Rescend();
            if (col.gameObject.GetComponent<DamageHandler>() != null)
            {
                col.gameObject.GetComponent<DamageHandler>().ApplyDamage(damage, gameObject, knockback);
            }
                
        }
    }
}
