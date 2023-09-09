using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cobweb : MonoBehaviour
{
    public ParticleSystem webParticles;
    public GameObject spider;

    public void Disintigrate(){
        ParticleSystem webPS = Instantiate(webParticles, transform.position, Quaternion.identity);
        webPS.transform.parent = transform.parent;
        webPS.Play();
        Destroy(gameObject);
    }
}
