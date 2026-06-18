using UnityEngine;

public class BombCollision : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadius = 5.0f;
    public float explosionForce = 500.0f;
    
    [Header("Friendly Fire")]
    [Tooltip("If true, this explosive will ignore collisions with the Biplane. Check this for player bombs, uncheck for tank rockets.")]
    public bool ignorePlayer = true;
    
    // We use a flag so it doesn't trigger multiple times in one frame
    private bool hasExploded = false;

    // Unity automatically calls this when a Rigidbody collides with another Collider
    void OnCollisionEnter(Collision collision)
    {
        if (hasExploded) return;

        // Check if we hit the Biplane (we use GetComponentInParent in case the collider is on a child object like a wing)
        if (ignorePlayer && collision.gameObject.GetComponentInParent<Biplane>() != null) 
        {
            return;
        }

        hasExploded = true;
        Explode();
    }

    void Explode()
    {
        // 1. Prototype Visual Effect (Creates an expanding red sphere)
        GameObject explosionVisual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        explosionVisual.transform.position = transform.position;
        
        // Remove the collider so our fake explosion doesn't physically push things
        Destroy(explosionVisual.GetComponent<Collider>());
        
        // Add a temporary script to handle the visual expansion and destruction
        explosionVisual.AddComponent<ExplosionEffect>();

        // 2. Physics Logic (Find everything in the blast radius)
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Add physical push back
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
            
            // Check if the object we hit has a Health component
            Health targetHealth = nearbyObject.GetComponentInParent<Health>();
            if (targetHealth != null)
            {
                // Calculate damage based on distance (closer = more damage)
                float distance = Vector3.Distance(transform.position, nearbyObject.transform.position);
                float damageMultiplier = 1.0f - (distance / explosionRadius);
                float damageToDeal = 50.0f * damageMultiplier; // Max 50 damage at the center
                
                targetHealth.TakeDamage(damageToDeal);
            }
        }

        // 3. Destroy the bomb
        Destroy(gameObject);
    }
}

// A simple helper class to animate our prototype explosion
public class ExplosionEffect : MonoBehaviour
{
    float lifetime = 0.4f;
    float age = 0.0f;
    
    void Start()
    {
        // Try to make it orange/red
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = new Color(1.0f, 0.4f, 0.0f); 
        }
    }

    void Update()
    {
        age += Time.deltaTime;
        if (age > lifetime)
        {
            Destroy(gameObject);
        }
        else
        {
            // Rapidly expand the sphere to look like a blast wave
            float scale = Mathf.Lerp(1.0f, 8.0f, age / lifetime);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}