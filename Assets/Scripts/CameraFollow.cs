using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2f;
    public Transform target;
    public Vector2 maxPosition = new Vector2(1000, 1000);
    public Vector2 minPosition = new Vector2(-1000, -1000);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y, -10f);

        if(newPos.x > maxPosition.x)
        {
            newPos.x = maxPosition.x;
        }
        if(newPos.y > maxPosition.y)
        {
            newPos.y = maxPosition.y;
        }
        if(newPos.x < minPosition.x)
        {
            newPos.x = minPosition.x;
        }
        if(newPos.y < minPosition.y)
        {
            newPos.y = minPosition.y;
        }

        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}
