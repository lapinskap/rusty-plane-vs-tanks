using UnityEngine;
using UnityEngine.SceneManagement; // Required for restarting the level

public class GameOverUI : MonoBehaviour
{
    [Tooltip("Drag the Game Over Panel here so we can turn it on when the player dies.")]
    public GameObject gameOverPanel;

    void Start()
    {
        // 1. Hide the panel when the game starts
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // 2. Find the player and listen for their death
        Biplane player = Object.FindAnyObjectByType<Biplane>();
        if (player != null)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.OnDeath.AddListener(ShowGameOver);
            }
        }
    }

    void ShowGameOver()
    {
        // Turn on the UI
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        // Optional: Slow down time for dramatic effect!
        // Time.timeScale = 0.5f; 
    }

    // This must be PUBLIC so the Unity UI Button can click it
    public void RestartGame()
    {
        // Reset time in case we changed it
        Time.timeScale = 1.0f; 
        
        // Reload the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}