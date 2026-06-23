using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    [Tooltip("Check this to make this specific object invincible.")]
    public bool isGodMode = false;
    
    [Tooltip("Optional: Link a SessionData file here to read global God Mode state (usually just for the Player).")]
    public SessionData sessionData;

    private float currentHealth;

    [Header("Events")]
    [Tooltip("Fired whenever health changes. Passes a percentage from 0.0 to 1.0.")]
    public UnityEvent<float> OnHealthChanged;
    
    [Tooltip("Fired when health reaches zero.")]
    public UnityEvent OnDeath;

    void Start()
    {
        // Start at full health
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        // Check if God Mode is on either locally, or globally via the session data
        bool effectiveGodMode = isGodMode || (sessionData != null && sessionData.isGodMode);
        
        if (effectiveGodMode || currentHealth <= 0) return; // Already dead or in god mode

        currentHealth -= amount;
        Debug.Log(gameObject.name + " took " + amount + " damage! Remaining: " + currentHealth);
        
        // Calculate the percentage (0 to 1) for UI health bars
        float healthPct = currentHealth / maxHealth;
        
        // Tell anyone listening (like our future UI) that health changed
        OnHealthChanged.Invoke(healthPct);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " was destroyed!");
        OnDeath.Invoke();
        
        // For now, we just destroy the object. We can make this fancier later.
        Destroy(gameObject);
    }
}