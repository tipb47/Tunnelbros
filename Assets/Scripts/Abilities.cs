using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    public GameObject heartPrefab;
    public Color doubleJumpColor = Color.blue;
    public Color shieldColor = Color.green;
    public Color punchColor = Color.red;

    private PlayerController playerController;

    void Start()
    {
        playerController = PlayerController.instance;


        //check the type of ability
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
            //check the type of ability
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
            //destroy afvter use
            Destroy(gameObject);
        }
    }

    void ActivateDoubleJump()
    {
        playerController.ActivateDoubleJump();
    }

    void ActivateShield()
    {
        playerController.ActivateShield(100f);  //Shield gives 100 extra
    }

    void ActivatePunch()
    {
        playerController.ActivatePunch();
    }
}
