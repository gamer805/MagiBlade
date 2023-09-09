using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    public bool criteriaMet = false;
    public bool open = false;

    public string sceneToLoad;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(open && Input.GetAxis("Fire2") > 0) SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Player" && criteriaMet){
            Open();
        }
    }

    void OnTriggerExit2D(Collider2D col){
        if(col.gameObject.tag == "Player"){
            Close();
        }
    }

    void Open(){
        if(!open){
            open = true;
            anim.SetTrigger("Open");
        }
    }
    void Close(){
        if(open){
            open = false;
            anim.SetTrigger("Close");
        }
    }


}
