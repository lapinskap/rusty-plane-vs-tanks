using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [Tooltip("Drag the Victory Panel here.")]
    public GameObject victoryPanel;

    void Start()
    {
        // 1. Hide the panel when the game starts
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        // 2. Listen for the Victory event from the Mission Manager
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.OnVictory.AddListener(ShowVictoryScreen);
        }
    }

    void ShowVictoryScreen()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
    }

    // Connect this to a "Play Again" or "Next Level" button
    public void RestartLevel()
    {
        Time.timeScale = 1.0f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
