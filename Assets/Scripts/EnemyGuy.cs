using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuy : MonoBehaviour
{

    private Animator animator;

    //movement
    public float moveSpeed = 2f;
    private bool movingRight = true;

    public AudioClip stompSound;
    private AudioSource audioSource;

    //turn
    [SerializeField]
    private float turnCooldown = 3f;  //cooldown time (in seconds)
    private float turnTimer = 0f;  //track when to turn

    private Rigidbody2D rb;

    [SerializeField] private LayerMask playerLayer;
    public int damageAmount = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Move();

        turnTimer += Time.deltaTime;

        if (turnTimer >= turnCooldown)
        {
            TurnAround();
            turnTimer = 0f;  //reset timer
        }
    }

    // move l/r
    void Move()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);  // right
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y); // left
        }

        UpdateAnimation(rb.velocity.magnitude);
    }

    // turn the Goomba around
    void TurnAround()
    {
        movingRight = !movingRight;  // toggle dir
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;  // flip sprite
        transform.localScale = localScale;
    }

    void UpdateAnimation(float moveInput)
    {
        animator.SetBool("isWalking", Mathf.Abs(moveInput) > 0.05f);
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
                Die();  // Enemy dies if stepped on
            }
            else
            {
                // Deal damage to the player
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(25);
            }
        }
    }

    void Die()
    {
        // Play death animation if any (optional)
        SoundEffects.instance.PlaySound(SoundEffects.instance.stompSound);

        // Disable enemy (or destroy after delay to allow animation to play)
        Destroy(gameObject); // Delay to let animation play out
    }
}
