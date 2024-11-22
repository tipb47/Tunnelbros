using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the tag "Player"
        if (collision.CompareTag("Player"))
        {
            // Call the GameOver method in the GameManager singleton
            GameManager.Instance.GameOver();
            Debug.Log("Player reached the end door! Game Over triggered.");
        }
    }
}
