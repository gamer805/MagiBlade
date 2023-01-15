using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Bow : MonoBehaviour
{
    Transform childArrow;
    public GameObject arrowPref;
    Projectile arrowStats;

    float reloadTime;

    bool reloading = false;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        childArrow = transform.GetChild(0);
        arrowStats = childArrow.GetComponent<Projectile>();
        reloadTime = arrowStats.reloadTime;
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {

        if (transform.childCount == 0 && !reloading)
        {
            reloading = true;
            StartCoroutine(arrowInstance());
        }
        if(childArrow != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.ResetTrigger("bounceBack");
                anim.ResetTrigger("Load");
                anim.SetTrigger("Load");
            }
            if (!arrowStats.rb.isKinematic)
            {
                anim.SetTrigger("bounceBack");
            }
        }
    }

    IEnumerator arrowInstance()
    {
        yield return new WaitForSeconds(reloadTime);
        GameObject arrow = Instantiate(arrowPref, transform.position, transform.rotation);
        arrow.transform.parent = gameObject.transform;
        childArrow = arrow.transform;
        arrowStats = childArrow.GetComponent<Projectile>();
        reloading = false;
    }
}
