﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;

    public void Load()
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
}
