using UnityEngine;
using UnityEngine.Events;
using TMPro; // For TextMeshPro UI
using UnityEngine.InputSystem;

public enum MissionType
{
    CollectRings,
    DestroyTanks
}

public class MissionManager : MonoBehaviour
{
    // A "Singleton" pattern. This allows any script to easily talk to the MissionManager
    // by calling MissionManager.Instance without needing to drag-and-drop references.
    public static MissionManager Instance { get; private set; }

    [Header("Mission Settings")]
    public MissionType missionType = MissionType.CollectRings;
    
    [Tooltip("Total number of rings or tanks required for the mission")]
    [UnityEngine.Serialization.FormerlySerializedAs("totalRings")]
    public int totalObjectives = 5;
    
    private int objectivesCompleted = 0;

    [Header("UI References")]
    [Tooltip("Drag the TextMeshPro text here to display progress (e.g., 'Rings: 0/5' or 'Tanks: 0/5')")]
    [UnityEngine.Serialization.FormerlySerializedAs("ringsText")]
    public TextMeshProUGUI progressText;
    
    [Tooltip("Drag a TextMeshPro text here to display instructions like 'Return to Base!'")]
    public TextMeshProUGUI instructionText;

    [Header("Events")]
    [Tooltip("Fired when the player completes all objectives (e.g., to turn on a spotlight at the base)")]
    [UnityEngine.Serialization.FormerlySerializedAs("OnAllRingsCollected")]
    public UnityEvent OnAllObjectivesCompleted;
    
    [Tooltip("Fired when the player reaches the base WITH all objectives completed")]
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
        
        // Initial instruction based on mission type
        if (instructionText != null)
        {
            if (missionType == MissionType.CollectRings)
                instructionText.text = "Fly through all loops!";
            else if (missionType == MissionType.DestroyTanks)
                instructionText.text = "Destroy all enemy tanks!";
        }

        var cheatVictoryAction = InputSystem.actions.FindAction("CheatVictory");
        cheatVictoryAction.performed += ctx => OnVictory.Invoke();
    }

    public void CollectRing()
    {
        if (missionType != MissionType.CollectRings) return;

        CompleteObjective("Ring collected!");
    }

    public void DestroyTank()
    {
        if (missionType != MissionType.DestroyTanks) return;

        CompleteObjective("Tank destroyed!");
    }

    private void CompleteObjective(string debugMessage)
    {
        objectivesCompleted++;
        UpdateUI();
        
        Debug.Log(debugMessage + " " + objectivesCompleted + "/" + totalObjectives);

        if (objectivesCompleted >= totalObjectives)
        {
            Debug.Log("All objectives completed! Return to base.");
            
            // Update the instruction text for the player
            if (instructionText != null)
            {
                instructionText.text = "MISSION UPDATE: Return to Base!";
                instructionText.color = Color.yellow; // Make it pop!
            }
            
            OnAllObjectivesCompleted.Invoke();
        }
    }

    private void UpdateUI()
    {
        if (progressText != null)
        {
            if (missionType == MissionType.CollectRings)
                progressText.text = $"Rings: {objectivesCompleted} / {totalObjectives}";
            else if (missionType == MissionType.DestroyTanks)
                progressText.text = $"Tanks: {objectivesCompleted} / {totalObjectives}";
        }
    }

    public void ReachFinishLine()
    {
        if (objectivesCompleted >= totalObjectives)
        {
            Debug.Log("Mission Accomplished! You Win!");
            Time.timeScale = 0f; // Freeze game
            OnVictory.Invoke();  // Tell the Victory UI to show up
        }
        else
        {
            // You could display a temporary warning on the HUD here!
            string objectiveName = missionType == MissionType.CollectRings ? "rings" : "tanks";
            Debug.Log($"You need {totalObjectives - objectivesCompleted} more {objectiveName} to finish!");
        }
    }
}
