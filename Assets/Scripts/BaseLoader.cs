using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseLoader : MonoBehaviour
{
    public string baseTitle = "Base";

    void Update(){
        if(Input.GetKeyDown(KeyCode.X)) Load();
    }
    void Load()
    {
        SceneManager.LoadScene(baseTitle, LoadSceneMode.Single);
    }
}
