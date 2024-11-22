using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //variables
    public float moveSpeed = 5f;
    private bool isFacingRight = true;
    private float horizontalInput;

    //hp
    private Image healthBar;
    private Image shieldBar;
    public float healthAmount = 100f;
    private bool isDead = false;

    //components
    private Rigidbody2D rb;
    private Animator animator;


    //jumping stuff
    public float jumpForce = 10f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded = false;
    private bool hasDoubleJumped = false;

    // Ability-relatedd
    private bool doubleJumpEnabled = false;
    private bool hasShield = false;
    private bool canPunch = false;
    private float shieldAmount = 100f;

    public static PlayerController instance;
    private UIManager manager;

    public GameObject attackPoint;
    public float attackRadius = 5f; // Radius around the attackPoint
    public LayerMask attackLayers;   // Layers to check for punch targets
    private Vector3 attackPointOffset; // Relative position of attackPoint

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize Rigidbody2D and Animator
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        manager = UIManager.manager;

        attackPointOffset = attackPoint.transform.localPosition;
    }

    void Update()
    {
        //Debug.Log($"Player Position: {transform.position}, AttackPoint Position: {attackPoint.transform.position}");

        // check if the player is grounded first
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);


        if (isGrounded)
        {
            hasDoubleJumped = false;
            animator.SetBool("isJumping", false);
        }

        // movement
        float horizontalInput = Input.GetAxis("Horizontal");
        Move(horizontalInput);

        // jump logic
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (doubleJumpEnabled && !hasDoubleJumped)
            {
                DoubleJump();
            }
        }

        // punch
        if (Input.GetButtonDown("Fire1") && canPunch)
        {
            Punch();
        }


        if (healthAmount <= 0 && !isDead) {
            isDead = true;
            GameManager.Instance.GameOver();
        }
    }

    void Move(float moveInput)
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));

        if (moveInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Jump()
    {
        animator.SetBool("isJumping", true);
        SoundEffects.instance.PlaySound(SoundEffects.instance.jumpSound);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void DoubleJump()
    {
        animator.SetBool("isJumping", true);
        SoundEffects.instance.PlaySound(SoundEffects.instance.jumpSound);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        hasDoubleJumped = true;
        Debug.Log("Double Jump Executed!");
    }

    void Flip()
    {
        // flip sprite
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void ActivateDoubleJump()
    {
        doubleJumpEnabled = true;
        Debug.Log("Double Jump Enabled: " + doubleJumpEnabled);
        Debug.Log("Double Jump Activated!");
    }

    public void ActivateShield(float shieldPoints)
    {
        hasShield = true;
        manager.shieldBar.enabled = true;
        shieldAmount = shieldPoints;
        Debug.Log("Shield Activated! Extra health: " + shieldPoints);
    }

    public void ActivatePunch()
    {
        canPunch = true;
        Debug.Log("Punch Ability Activated!");
    }

    void Punch()
    {
        animator.SetTrigger("isPunching");
        SoundEffects.instance.PlaySound(SoundEffects.instance.punchSound);

        // check for colliders within the radius of the attackPoint
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRadius, attackLayers);

        foreach (Collider2D hit in hits)
        {
            Debug.Log($"Hit object: {hit.name}");
            if (hit.CompareTag("Enemy") || hit.CompareTag("Box"))
            {
                Destroy(hit.gameObject); // destroy enemy/box
                Debug.Log($"{hit.tag} punched!");
            }
        }
    }


    public void TakeDamage(float damage)
    {
        SoundEffects.instance.PlaySound(SoundEffects.instance.damageSound);
        if (hasShield)
        {
            shieldAmount -= damage;
            manager.shieldBar.fillAmount = shieldAmount / 100f;

            if (shieldAmount <= 0)
            {
                hasShield = false;
                manager.shieldBar.enabled = false;
                Debug.Log("Shield broken!");
            }
        }
        else
        {
            healthAmount -= damage;
            manager.healthBar.fillAmount = healthAmount / 100f;

            if (healthAmount <= 0)
            {
                Debug.Log("Player is dead! Triggering Game Over.");
            }
        }
    }


    public void Heal(float healing)
    {
        if (hasShield)
        {
            shieldAmount += healing;
            shieldAmount = Mathf.Clamp(shieldAmount, 0, 100);

            manager.shieldBar.fillAmount = shieldAmount / 100f;
        }
        else
        {
            healthAmount += healing;
            healthAmount = Mathf.Clamp(healthAmount, 0, 100);

            manager.healthBar.fillAmount = healthAmount / 100f;
        }
    }
}
