using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paralaxBack : MonoBehaviour
{
    Transform target;

    public Vector3 offset;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        transform.position = target.position + offset;
    }
}
