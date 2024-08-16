using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] public float playerSpeed = 0.025f;
    [SerializeField] public float jumpForce = 0.025f;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isFacingRight = true;
    public bool isGrounded = false;
    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(Time.deltaTime * playerSpeed, 0f, 0f));
            this.isFacingRight = true;
            if (isGrounded )
            {
                animator.SetBool("isRunning", true);
            }
        }
        if (Input.GetKey(KeyCode.A)){
            this.isFacingRight = false;
            transform.Translate(new Vector3(Time.deltaTime * -playerSpeed, 0f, 0f));
            if (isGrounded)
            {
                animator.SetBool("isRunning", true);
            }
        }
        if (Input.GetKey(KeyCode.W) && isGrounded)
        {
            this.rigidBody.AddForce(new Vector3(0f, jumpForce, 0f));
            isGrounded = false;
        }
        if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            animator.SetBool("isRunning", false);
        }
        this.spriteRenderer.flipX = !isFacingRight;
        animator.SetBool("isGrounded", isGrounded);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
    }
}
