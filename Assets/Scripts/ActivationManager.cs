using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationManager : MonoBehaviour
{
    public void Activate() {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    public void Deactivate() {
        gameObject.SetActive(false);
    }
}
