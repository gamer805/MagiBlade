using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[HideInInspector] public Rigidbody2D rb;

	[HideInInspector] public Vector3 pos { get { return transform.position; } }

	public GameObject gameManager;
	bool usedAbility = false;
	ProjectileManager managerScript;

	public float damage;
	public float knockback;
	public string source;
	public GameObject resultPrefab;
	public float reloadTime;

	public Color color;
	public Color reloadingColor;
	public float duration = 2f;
	public float redux = 0.2f;
	public string effect;
	public bool aim = false;
	public float aimOffset = 0f;

	public float deletionTime = 2f;
	float deleteTimer = 0f;
	bool deleting = false;

	bool damageApplied = false;

	[HideInInspector] public bool used = false;

	public Transform com;

	void Start ()
	{
		rb = GetComponent<Rigidbody2D>();
		DesactivateRb();
		managerScript = gameManager.GetComponent<ProjectileManager>();
		rb.centerOfMass = com.position;
	}

	void Update()
    {
        if (aim && Input.GetMouseButton(0))
        {
			LookAtMouse();
        }
		if(deleting == true && deleteTimer == deletionTime){
			DeleteSpell();
		}
		if(deleting){
			deleteTimer -= Time.deltaTime;
		}

    }

	public void LookAtMouse()
    {
		Vector3 mouse_pos = Input.mousePosition;
		mouse_pos.z = -10f; //The distance between the CameraManager and object
		Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
		mouse_pos.x = mouse_pos.x - object_pos.x;
		mouse_pos.y = mouse_pos.y - object_pos.y;
		float angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + aimOffset));
	}

	public void Push (Vector2 force)
	{
		rb.AddForce (force, ForceMode2D.Impulse);
		deleting = true;
	}

	public void ActivateRb ()
	{
		rb.isKinematic = false;
		foreach (Collider2D col in gameObject.GetComponents<Collider2D>())
		{
			col.enabled = true;
		}
		used = true;
	}

	public void DesactivateRb ()
	{
		rb.velocity = Vector3.zero;
		rb.angularVelocity = 0f;
		rb.isKinematic = true;
        foreach (Collider2D col in gameObject.GetComponents<Collider2D>())
        {
			col.enabled = false;
        }
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag != source && damageApplied == false)
        {
			damageApplied = true;
			Debug.Log(col.gameObject);
			//scripts
			DamageHandler dmgScript = col.gameObject.GetComponent<DamageHandler>();
			spellEffector spellScript = col.gameObject.GetComponent<spellEffector>();

			//spell
			if (spellScript != null)
            {
				//damage w/o color
				if (dmgScript != null)
					dmgScript.AppDmgNoColor(damage, gameObject, knockback);

				//apply effect
				if (effect == "freeze")
                {
					spellScript.Freeze(color, duration, redux);
				} else if (effect == "weaken")
                {
					spellScript.Weaken(color, duration, redux);
                }
            } else
            {
				//damage w/ color
				if (dmgScript != null)
					dmgScript.ApplyDamage(damage, gameObject, knockback);
			}

			//spawning
			if (resultPrefab != null)
				Instantiate(resultPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), Quaternion.identity);

			//discontinuation
			Invoke("DeleteSpell", 0.05f);
        }
	}

	void DeleteSpell()
    {
		Debug.Log(transform.position);
		Destroy(gameObject);
    }
}
