using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [SerializeField] public float playerSpeed = 5f;
    [SerializeField] public float jumpForce = 10f;
    [SerializeField] public float jumpPadForce = 11;
    [SerializeField] public float maxSpeed = 10f;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isFacingRight = true;
    public bool isGrounded = false;
    private Rigidbody2D rigidBody;
    private Vector2 velocity;
    private bool isDead = false;
    public float respawnDelay = 2f;
    private Vector2 spawnPoint = new Vector2(-7.5f, -2.4725f);
    private ParticleSystem particleSystem;

    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip jumpPadClip;
    [SerializeField] private AudioClip[] walkClips;
    [SerializeField] private AudioClip dieClip;

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
        if(!isDead) {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");

            rigidBody.velocity = new Vector2(moveHorizontal * playerSpeed, rigidBody.velocity.y);
            rigidBody.velocity = Vector2.ClampMagnitude(rigidBody.velocity, maxSpeed);

            if((rigidBody.velocity.x > 0 || rigidBody.velocity.x < 0) && isGrounded) {
                SoundFXManager.instance.PlayRandomSoundFXClip(walkClips, transform, 1f);
            }
            
            if (Input.GetKeyDown(KeyCode.W) && isGrounded) {
                SoundFXManager.instance.PlaySoundFXClip(jumpClip, transform, 1f);
                rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isGrounded = false;
            }

            animator.SetBool("isRunning", moveHorizontal != 0);
            animator.SetBool("isGrounded", isGrounded);

            if (moveHorizontal > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (moveHorizontal < 0 && isFacingRight)
            {
                Flip();
            }
        } else {
            animator.SetBool("isDead", true);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !isFacingRight;
    }

    public void Die()
    {
        SoundFXManager.instance.PlaySoundFXClip(dieClip, transform, 1f);
        if (isDead) return; // Prevent multiple calls to Die()
        isDead = true;
        Invoke("RespawnOrGameOver", respawnDelay);
    }

    private void RespawnOrGameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Respawn()
    {
        isDead = false;
        transform.position = spawnPoint;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Jump Pad") {
            SoundFXManager.instance.PlaySoundFXClip(jumpPadClip, transform, 1f);
            rigidBody.AddForce(Vector2.up * jumpPadForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Spike") {
            Die();
        } else {
            isGrounded = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isGrounded = false;
        // this.velocity.y = 0;
    }
}
