using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //variables
    public float moveSpeed = 5f;
    private bool isFacingRight = true;

    //hp
    private Image healthBar;
    private Image shieldBar;
    private float healthAmount = 100f;
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
    private GameObject punchPrefab;

    public static PlayerController instance;
    private UIManager manager;

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

    }

    void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Reset double jump when player lands
        if (isGrounded)
        {
            hasDoubleJumped = false;  // Allows double jump to reset on landing
        }

        // Horizontal movement
        float moveInput = Input.GetAxis("Horizontal");
        Move(moveInput);
        UpdateAnimation(moveInput);

        // Jump logic
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                Jump();  // Normal jump
            }
            else if (doubleJumpEnabled && !hasDoubleJumped)
            {
                DoubleJump();  // Double jump
            }
        }

        // Punch input
        if (Input.GetButtonDown("Fire1") && canPunch)
        {
            Punch();
        }
    }


    void Move(float moveInput)
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        //flip if moving to the left
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
        SoundEffects.instance.PlaySound(SoundEffects.instance.jumpSound);
        // apply upward force, enable jump animation
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void DoubleJump()
    {
        SoundEffects.instance.PlaySound(SoundEffects.instance.jumpSound);
        // Apply upward force for double jump
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        hasDoubleJumped = true;  // Mark double jump as used
        Debug.Log("Double Jump Executed!");
    }


    void Flip()
    {
        //flip direction of sprite animations
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void UpdateAnimation(float moveInput)
    {
        //velocity check
        if (Mathf.Abs(moveInput) > 0.05f)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
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
        animator.SetTrigger("Punch");
        SoundEffects.instance.PlaySound(SoundEffects.instance.punchSound);
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
                isDead = true;
                Debug.Log("died!");
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
