using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semisolid : MonoBehaviour
{
    private GameObject playerRef;
    public AudioSource WoodThunk;

    [SerializeField] private Collider2D platformCollider;

    void Start() {
        platformCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.GetAxis("Vertical") == -1f && playerRef != null) {
            StartCoroutine(DisableCollision());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRef = collision.gameObject;
            WoodThunk.Play();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            playerRef = null;
        }
    }

    private IEnumerator DisableCollision()
    {

        Physics2D.IgnoreLayerCollision(platformCollider.gameObject.layer, LayerMask.NameToLayer("Player"));
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(platformCollider.gameObject.layer, LayerMask.NameToLayer("Player"), false);
    }
}