using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Tooltip("Drag the Main Menu Panel here.")]
    public GameObject mainMenuPanel;

    [Tooltip("Drag the HUD/Gameplay UI Panel here (optional) to hide it during menu.")]
    public GameObject gameplayHUDPanel;

    void Start()
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        // 1. Show the Main Menu
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }

        // 2. Hide the Gameplay HUD (if we have one linked)
        if (gameplayHUDPanel != null)
        {
            gameplayHUDPanel.SetActive(false);
        }
        
        // 3. Pause the game so the plane doesn't fly away
        Time.timeScale = 0f; 
    }

    public void StartSurvivalMode()
    {
        SetPlayerGodMode(false);
        StartGameSequence();
    }

    public void StartGodMode()
    {
        SetPlayerGodMode(true);
        StartGameSequence();
    }

    private void SetPlayerGodMode(bool godModeEnabled)
    {
        // Find the player in the scene
        Biplane player = Object.FindAnyObjectByType<Biplane>();
        if (player != null)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.isGodMode = godModeEnabled;
                if (godModeEnabled)
                {
                    Debug.Log("God Mode Enabled: Player will not take damage.");
                }
            }
        }
    }

    // This handles the common logic of hiding the menu and unpausing
    private void StartGameSequence()
    {
        // 1. Hide the Main Menu
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }

        // 2. Show the Gameplay HUD
        if (gameplayHUDPanel != null)
        {
            gameplayHUDPanel.SetActive(true);
        }

        // 3. Resume time so the game starts
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game requested.");
        Application.Quit(); // Note: This doesn't do anything in the Unity Editor, only in built games
    }
}
