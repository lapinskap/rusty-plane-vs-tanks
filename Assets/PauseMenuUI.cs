using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [Tooltip("Drag the Pause Menu Panel here.")]
    public GameObject pauseMenuPanel;

    private bool isPaused = false;

    void Start()
    {
        // Ensure it's hidden when the scene loads
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Listen for the Escape key to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        // Safety check: Don't open pause menu if time is ALREADY stopped 
        // (meaning we are in the Main Menu or Game Over screen)
        if (Time.timeScale == 0f && !isPaused) 
        {
            return; 
        }

        isPaused = true;
        Time.timeScale = 0f; // Freeze the game
        
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }

        Debug.Log("Game Paused. Press Resume or Escape to continue.");
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Unfreeze the game
        
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        Debug.Log("Game Resumed.");
    }

    public void ReturnToMainMenu()
    {
        // 1. Unfreeze time (crucial before loading a scene)
        Time.timeScale = 1f;

        // 2. Reload the scene. 
        // Since MainMenuUI runs its Start() method on scene load, 
        // reloading the scene will reset all gameplay and show the main menu again!
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Debug.Log("Returning to Main Menu...");
    }
}
