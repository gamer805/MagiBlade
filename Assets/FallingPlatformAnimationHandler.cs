using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformAnimationHandler : MonoBehaviour
{
    FallingPlatform fallingPlatform;
    // Start is called before the first frame update
    void Start()
    {
        fallingPlatform = transform.parent.GetComponent<FallingPlatform>();
    }

    public void TriggerFall() {
        fallingPlatform.Fall();
    }
}
