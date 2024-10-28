using UnityEngine;

public class Waste : MonoBehaviour
{
    public float radioactivityLevel = 5f;  // Radioactivity level for this waste item

    private UIManager uiManager; // Reference to UIManager
    private bool hasBeenCollected = false; // Flag to track if waste has been collected

    void Start()
    {
        // Find and assign the UIManager instance
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has collected the waste
        if (other.CompareTag("Player") && !hasBeenCollected)
        {
            hasBeenCollected = true; // Prevents double removal

            // Remove waste from the UIManager's count
            if (uiManager != null)
            {
                uiManager.RemoveWaste(radioactivityLevel);
            }

            Destroy(gameObject); // Destroy waste after collection
        }
    }

    private void OnDestroy()
    {
        // Only call RemoveWaste if it hasn't been collected already
        if (!hasBeenCollected && uiManager != null)
        {
            uiManager.RemoveWaste(radioactivityLevel);
        }
    }
}
