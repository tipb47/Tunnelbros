using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of vertical movement
    public float moveDistance = 10f; // Distance the enemy travels up and down

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
        // Move the enemy up and down
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
            // Get the collision point
            Vector2 contactPoint = collision.GetContact(0).point;
            Vector2 enemyCenter = transform.position;

            // Check if the collision point is above the enemy's center
            if (contactPoint.y > enemyCenter.y + 0.175f) // threshold for 'head'
            {
                // Make the player bounce
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    playerRb.velocity = new Vector2(playerRb.velocity.x, 10f); // Adjust bounce height as needed
                }

                Die();  // Enemy dies if stepped on
            }
            else
            {
                // Deal damage to the player
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
