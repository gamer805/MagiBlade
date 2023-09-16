using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DependencyHandler : MonoBehaviour
{
    public bool dependencyExists = true;
    public void UseBehavior () {
        dependencyExists = false;
    }
}