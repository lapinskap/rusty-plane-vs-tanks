using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    [Tooltip("Drag the Victory Panel here.")]
    public GameObject victoryPanel;

    void Awake()
    {
       // Decided to use Inspector assignment for the victoryPanel instead of finding it in code. This is more efficient and avoids potential null reference issues.
    }

    void Start()
    {
        // 1. Hide the panel when the game starts
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    public void ShowVictoryScreen()
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

    public void NextLevel()
    {
        Time.timeScale = 1.0f; 
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
