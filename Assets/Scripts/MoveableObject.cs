using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public Rigidbody2D rb;
    public BoxCollider2D boxCollider;
    public float followSpeed = 5f;
    public float destroyDuration = 0.5f;
    [SerializeField] private AudioClip disintegrateClip;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Move(Vector2 targetPosition)
    {
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, followSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Jump Pad"))
        {
            rb.AddForce(Vector2.up * 11, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Disintegration Grid"))
        {
            SoundFXManager.instance.PlaySoundFXClip(disintegrateClip, transform, 1f);
            StartCoroutine(ScaleDownAndDestroy());
        }
    }

    private IEnumerator ScaleDownAndDestroy() {
        Vector2 originalScale = transform.localScale;
        Vector2 targetScale = Vector2.one * 0.01f;
        float elapsedTime = 0f;
        float duration = destroyDuration;

        while (elapsedTime < duration) {
            transform.localScale = Vector2.Lerp(originalScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;

        Destroy(gameObject);
    }
}
