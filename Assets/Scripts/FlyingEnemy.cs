using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float moveSpeed = 2f; // vertical movement
    public float moveDistance = 10f; // distance up and down

    private AudioSource audioSource;
    public AudioClip stompSound;

    private Vector3 startPos;
    private bool movingUp = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (movingUp)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            if (transform.position.y >= startPos.y + moveDistance)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            if (transform.position.y <= startPos.y - moveDistance)
            {
                movingUp = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 contactPoint = collision.GetContact(0).point;
            Vector2 enemyCenter = transform.position;

            // check if above head
            if (contactPoint.y > enemyCenter.y + 0.175f) // threshold for 'head'
            {
                // if so lets bounce!
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, 10f);
                }

                Die();
            }
            else
            {
                // else it was a take damage situation
                PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(25);
                }
            }
        }
    }


    void Die()
    {
        SoundEffects.instance.PlaySound(SoundEffects.instance.stompSound);
        // Disable enemy (or destroy after delay to allow animation to play)
        Destroy(gameObject); // Delay to let animation play out
    }
}
