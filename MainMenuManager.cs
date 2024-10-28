using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Method to start the game by loading the Game scene
    public void StartGame()
    {
        // Replace "GameScene" with the actual name of your main game scene
        SceneManager.LoadScene("aquaticreactor");
    }

    // Method to quit the game
    public void QuitGame()
    {
        // Logs a message when quitting in the editor (for testing)
        Debug.Log("Quit Game");
        Application.Quit(); // Quits the application (only works in build)
    }
}
