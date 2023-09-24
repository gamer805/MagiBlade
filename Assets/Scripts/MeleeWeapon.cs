using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public float damage;
    public float knockbackPower;
    public LayerMask targetLayer;

    public float reloadTime;
    public float extensionTime = 0.1f;

    float reloadTimer;
    float extensionTimer;

    public Color reloadCol;
    public Color defaultCol;
    

    public bool canDamage = false;
    public bool canSwing = true;

    public Vector3 offset;

    Animator anim;
    SpriteRenderer renderer;
    public AudioSource attackSound;

    // Start is called before the first frame update
    void Start()
    {
        reloadTimer = reloadTime;
        extensionTimer = extensionTime;
        transform.localPosition = offset;
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        reloadTimer += Time.deltaTime;
        extensionTimer += Time.deltaTime;

        if(reloadTimer < reloadTime) {
            renderer.color = reloadCol;
            canSwing = false;
        } else
        {
            renderer.color = defaultCol;
            canSwing = true;
        }

        if (extensionTimer < extensionTime) {
            canDamage = true;
        } else {
            canDamage = false;
        }

        if (Input.GetButtonDown("Fire1") && canSwing)
        {
            reloadTimer = 0;
            extensionTimer = 0;
            anim.SetTrigger("Used");
            if(attackSound != null) attackSound.Play();
        }
        
    }

    
    void OnTriggerStay2D(Collider2D col)
    {
        if (( targetLayer & (1 << col.gameObject.layer)) != 0 && canDamage)
        {
            
            if (col.gameObject.GetComponent<DamageHandler>() != null) {
                col.gameObject.GetComponent<DamageHandler>().ApplyDamage(damage, gameObject, knockbackPower);
            }
                
        }
    }
    
}
