using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    public Rigidbody2D rigidbody;
    public BoxCollider2D boxCollider;
    public bool isBeingMoved = false;
    public Vector2 initialOffset;
    public float moveSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        screenPosition = Input.mousePosition;
        screenPosition.z = 1f;

        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        if (!Input.GetKey(KeyCode.Space))
        {
            isBeingMoved = false;
        }
        if (boxCollider.OverlapPoint(new Vector2(worldPosition.x, worldPosition.y)) && Input.GetKey(KeyCode.Space) && !isBeingMoved)
        {
            Vector3 objPosition = transform.position;
            isBeingMoved = true;
            initialOffset = new Vector2(objPosition.x-worldPosition.x, objPosition.y - worldPosition.y);
        }

        if (isBeingMoved)
        {
            transform.position = Vector3.MoveTowards(new Vector3(transform.position.x + initialOffset.x, transform.position.y + initialOffset.y, 0f), worldPosition, moveSpeed);
            
            /*
            Vector2 distanceToMouse = new Vector2(worldPosition.x - (transform.position.x + initialOffset.x), worldPosition.y - (transform.position.y + initialOffset.y));
            float force = 100;
            float scaleFactor = (distanceToMouse.magnitude / 10) * Mathf.Sqrt(Mathf.Pow(force, 2) / (Mathf.Pow(distanceToMouse.x, 2) + Mathf.Pow(distanceToMouse.y, 2)));

            rigidbody.AddForce(new Vector3(scaleFactor * distanceToMouse.x, scaleFactor * distanceToMouse.y, 0f), ForceMode2D.Impulse);

            //if(distanceToMouse.magnitude <= 50)
            //{
              //  transform.position = new Vector3(worldPosition.x + initialOffset.x, worldPosition.y + initialOffset.y);
            //} */
        }

    }
}
