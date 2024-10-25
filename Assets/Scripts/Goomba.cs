using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    //movement
    public float moveSpeed = 2f;
    private bool movingRight = true;

    //turn
    [SerializeField]
    private float turnCooldown = 3f;  //cooldown time (in seconds)
    private float turnTimer = 0f;  //track when to turn

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    // turn the Goomba around
    void TurnAround()
    {
        movingRight = !movingRight;  // toggle dir
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;  // flip sprite
        transform.localScale = localScale;
    }
}

