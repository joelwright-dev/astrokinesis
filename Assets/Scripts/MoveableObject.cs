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
    public float followSpeed = 5;

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
        screenPosition.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (!Input.GetKey(KeyCode.Space))
        {
            isBeingMoved = false;
        }
        if (boxCollider.OverlapPoint(worldPosition) && Input.GetKey(KeyCode.Space) && !isBeingMoved)
        {
            isBeingMoved = true;
            initialOffset = (Vector2)transform.position - (Vector2)worldPosition;
        }
    }

    void FixedUpdate() {
        if (isBeingMoved)
        {
            Vector2 targetPosition = (Vector2)worldPosition + initialOffset;
            Vector2 newPosition = Vector2.MoveTowards(rigidbody.position, targetPosition, followSpeed * Time.fixedDeltaTime);
            rigidbody.MovePosition(newPosition);
        }
    }
}
