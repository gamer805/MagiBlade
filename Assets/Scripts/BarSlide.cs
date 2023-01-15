using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarSlide : MonoBehaviour
{
    Vector2 startPos;
    public Vector2 hiddenPos;

    public float moveSpeed = 3f;
    public float minimumDistance = 0.01f;

    bool hidden = false;
    bool moving = false;
    RectTransform rectTrans;

    // Start is called before the first frame update
    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        startPos = rectTrans.anchoredPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || moving){
            if(hidden) {
                Slide(minimumDistance, startPos, moveSpeed);
            } else {
                Slide(minimumDistance, hiddenPos, moveSpeed);
            }
        }
    }

    void Slide(float minDist, Vector2 goal, float speed){
        moving = true;
        if(Vector2.Distance(rectTrans.anchoredPosition, goal) > minDist){
            rectTrans.anchoredPosition = Vector2.MoveTowards(rectTrans.anchoredPosition, goal, speed*Time.deltaTime);
        } else {
            moving = false;
            hidden = !hidden;
        }
        
    }
}
