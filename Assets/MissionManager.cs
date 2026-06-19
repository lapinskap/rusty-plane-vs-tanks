using UnityEngine;
using UnityEngine.Events;
using TMPro; // For TextMeshPro UI

public class MissionManager : MonoBehaviour
{
    // A "Singleton" pattern. This allows any script to easily talk to the MissionManager
    // by calling MissionManager.Instance without needing to drag-and-drop references.
    public static MissionManager Instance { get; private set; }

    [Header("Mission Settings")]
    public int totalRings = 5;
    private int ringsCollected = 0;

    [Header("UI References")]
    [Tooltip("Drag the TextMeshPro text here to display 'Rings: 0/5'")]
    public TextMeshProUGUI ringsText;
    
    [Tooltip("Drag a TextMeshPro text here to display instructions like 'Return to Base!'")]
    public TextMeshProUGUI instructionText;

    [Header("Events")]
    [Tooltip("Fired when the player gets all rings (e.g., to turn on a spotlight at the base)")]
    public UnityEvent OnAllRingsCollected;
    
    [Tooltip("Fired when the player reaches the base WITH all rings")]
    public UnityEvent OnVictory;

    void Awake()
    {
        // Set up the Singleton
        if (Instance == null) 
        {
            Instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateUI();
        
        // Initial instruction
        if (instructionText != null)
        {
            instructionText.text = "Fly through all loops!";
        }
    }

    public void CollectRing()
    {
        ringsCollected++;
        UpdateUI();
        
        Debug.Log("Ring collected! " + ringsCollected + "/" + totalRings);

        if (ringsCollected >= totalRings)
        {
            Debug.Log("All rings collected! Return to base.");
            
            // Update the instruction text for the player
            if (instructionText != null)
            {
                instructionText.text = "MISSION UPDATE: Return to Base!";
                instructionText.color = Color.yellow; // Make it pop!
            }
            
            OnAllRingsCollected.Invoke();
        }
    }

    private void UpdateUI()
    {
        if (ringsText != null)
        {
            ringsText.text = $"Rings: {ringsCollected} / {totalRings}";
        }
    }

    public void ReachFinishLine()
    {
        if (ringsCollected >= totalRings)
        {
            Debug.Log("Mission Accomplished! You Win!");
            Time.timeScale = 0f; // Freeze game
            OnVictory.Invoke();  // Tell the Victory UI to show up
        }
        else
        {
            // You could display a temporary warning on the HUD here!
            Debug.Log($"You need {totalRings - ringsCollected} more rings to finish!");
        }
    }
}
