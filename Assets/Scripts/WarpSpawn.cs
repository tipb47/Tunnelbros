using UnityEngine;

public class WarpSpawn : MonoBehaviour
{
    public Vector3 warpTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // check if the player hit the door
        if (other.CompareTag("Player"))
        {
            other.transform.position = warpTarget;
        }
    }
}
