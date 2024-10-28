using UnityEngine;

public class OutOfWaterZone : MonoBehaviour
{
    // When the player enters the Out of Water Zone (top of the aquarium)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Debug.Log("Player entered the Out of Water Zone.");
                playerController.ApplyHighGravity();  // Apply high gravity when entering out of water
            }
        }
    }

    // When the player exits the Out of Water Zone and goes back into the water
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                Debug.Log("Player exited the Out of Water Zone (entered water).");
                playerController.ApplyLowGravity();  // Apply low gravity when returning to water
            }
        }
    }
}

