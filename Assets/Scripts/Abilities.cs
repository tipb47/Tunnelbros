using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public GameObject heartPrefab;
    public Color doubleJumpColor = Color.blue;
    public Color shieldColor = Color.green;
    public Color punchColor = Color.red;

    public GameObject doubleJumpDoor; 
    public GameObject punchDoor;      

    private PlayerController playerController;

    void Start()
    {
        playerController = PlayerController.instance;

        // Check the type of ability
        switch (gameObject.tag)
        {
            case "DoubleJump":
                heartPrefab.GetComponent<SpriteRenderer>().color = doubleJumpColor;
                break;
            case "Shield":
                heartPrefab.GetComponent<SpriteRenderer>().color = shieldColor;
                break;
            case "Punch":
                heartPrefab.GetComponent<SpriteRenderer>().color = punchColor;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (gameObject.tag)
            {
                case "DoubleJump":
                    ActivateDoubleJump();
                    break;
                case "Shield":
                    ActivateShield();
                    break;
                case "Punch":
                    ActivatePunch();
                    break;
            }
            Destroy(gameObject);
        }
    }

    void ActivateDoubleJump()
    {
        playerController.ActivateDoubleJump();
        if (doubleJumpDoor != null)
        {
            doubleJumpDoor.SetActive(true);
        }

        NotificationManager.Instance.ShowNotification("Double Jump Ability Acquired!", 3f);
    }

    void ActivateShield()
    {
        playerController.ActivateShield(100f);

        NotificationManager.Instance.ShowNotification("Shield Ability Acquired!", 3f);
    }

    void ActivatePunch()
    {
        playerController.ActivatePunch();
        if (punchDoor != null)
        {
            punchDoor.SetActive(true);
        }

        NotificationManager.Instance.ShowNotification("Punch Ability Acquired! (Left Click)", 3f);
    }
}
