using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreenShake : MonoBehaviour
{
    float shakeTimeRemaining, shakePower, shakeFalter;

    // Update is called once per frame
    void LateUpdate()
    {
        if(shakeTimeRemaining > 0){
            shakeTimeRemaining -= Time.deltaTime;

            Vector3 shakeAmt = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            shakeAmt *= shakePower;
            transform.position += shakeAmt;

            shakePower = Mathf.MoveTowards(shakePower, 0, shakeFalter*Time.deltaTime);
        }
    }

    public void Shake(float length, float power){
        shakeTimeRemaining = length;
        shakePower = power;

        shakeFalter = power / length;
    }
}
