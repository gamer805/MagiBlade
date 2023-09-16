using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DependencyHandler))]
public class Cobweb : MonoBehaviour
{

    public ParticleSystem webParticles;
    DependencyHandler dependencyHandler;

    bool hasDisintegrated = false;

    void Start() {
        dependencyHandler = GetComponent<DependencyHandler>();
    }

    void Update() {
        if (!hasDisintegrated && !dependencyHandler.dependencyExists) {
            hasDisintegrated = true;
            Disintigrate();
        }
    }

    public void Disintigrate(){
        ParticleSystem webPS = Instantiate(webParticles, transform.position, Quaternion.identity);
        webPS.transform.parent = transform.parent;
        webPS.Play();
        Destroy(gameObject);
    }
}
