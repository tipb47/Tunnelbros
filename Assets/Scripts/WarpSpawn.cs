using UnityEngine;

public class WarpSpawn : MonoBehaviour
{
    public Vector3 warpTarget; // Target position to warp the player

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Warp the player to the target position
            other.transform.position = warpTarget;
        }
    }
}
