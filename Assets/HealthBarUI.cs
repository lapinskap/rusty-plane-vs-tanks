using UnityEngine;
using UnityEngine.UI; // Required when working with Unity UI elements

public class HealthBarUI : MonoBehaviour
{
    [Tooltip("Drag the Fill Image here")]
    public Image fillImage;
    
    [Tooltip("Optional: Drag the Health script you want to track here. If empty, it will auto-detect.")]
    public Health targetHealth;

    void Start()
    {
        // 1. If we didn't manually assign a target, try to find one on this object or its parents (for the Tank)
        if (targetHealth == null)
        {
            targetHealth = GetComponentInParent<Health>();
        }

        // 2. If STILL null, fallback to finding the Biplane player (for the main screen UI)
        if (targetHealth == null)
        {
            Biplane player = Object.FindAnyObjectByType<Biplane>();
            if (player != null)
            {
                targetHealth = player.GetComponent<Health>();
            }
        }

        // 3. Subscribe to the event
        if (targetHealth != null)
        {
            targetHealth.OnHealthChanged.AddListener(UpdateFill);
            
            // Force the UI to update once immediately so we don't start with an empty bar
            UpdateFill(1.0f); 
        }
    }

    // This method will be called automatically whenever the target's health changes
    void UpdateFill(float percentage)
    {
        if (fillImage != null)
        {
            // The fillAmount goes from 0.0 (empty) to 1.0 (full)
            fillImage.fillAmount = percentage;

            // Change the color from Green to Red as health drops!
            fillImage.color = Color.Lerp(Color.red, Color.green, percentage);
        }
    }
}