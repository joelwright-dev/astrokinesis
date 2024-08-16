using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] public float playerSpeed = 0.025f;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            this.isFacingRight = true;
            transform.Translate(new Vector3(playerSpeed, 0f, 0f));
            animator.SetBool("isRunning", true);
        }else if (Input.GetKey(KeyCode.A)){
            this.isFacingRight = false;
            transform.Translate(new Vector3(-playerSpeed, 0f, 0f));
            animator.SetBool("isRunning", true);
        }else if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isGrounded", false);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
        this.spriteRenderer.flipX = !isFacingRight;
    }
}
