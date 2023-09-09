using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Transform target;

    float smoothSpeed = 0.125f;

    public Vector3 offset;

    public bool bounds;

    public Vector3 minPos;
    public Vector3 maxPos;

    public float height;
    public float width;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        
    }

    void FixedUpdate()
    {
        if(target == null){
            target = GameObject.FindWithTag("Player").transform;
        }
        if(target != null){
            Vector3 desiredPos = target.position + offset;
            if (bounds)
            {
                desiredPos.x = Mathf.Clamp(desiredPos.x, minPos.x, maxPos.x);
                desiredPos.y = Mathf.Clamp(desiredPos.y, minPos.y, maxPos.y);
            }
            Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);
            transform.position = smoothPos;
        }
        

        
        
    }

    void OnDrawGizmosSelected()
    {
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
        Gizmos.DrawWireCube(new Vector3((maxPos.x+minPos.x)/2, (maxPos.y + minPos.y)/2, 0), new Vector3(width + (maxPos.x - minPos.x), height + (maxPos.y - minPos.y), 1));
    }
}
