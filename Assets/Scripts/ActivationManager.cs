using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationManager : MonoBehaviour
{
    public void Activate()
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
