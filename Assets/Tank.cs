using UnityEngine;

public class Tank : MonoBehaviour
{
    public GameObject rocket;   
    
    Vector3 destination;
    UnityEngine.AI.NavMeshAgent agent;
    Transform tower;
    Transform playerTarget; // Reference to our player
    Rigidbody playerRigidbody; // Needed to get the plane's velocity

    float timeUntilWander = 0.0f;
    float timeUntilFire = 10.0f; // Separate timer for shooting
    float rocketSpeed = 45.0f; // Extracted to a variable for prediction math
    

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        tower = transform.Find("Tower");
        
        // Find the player dynamically at the start of the game
        Biplane biplane = Object.FindAnyObjectByType<Biplane>();
        if (biplane != null)
        {
            playerTarget = biplane.transform;
            playerRigidbody = biplane.GetComponent<Rigidbody>();
        }
    }

    // Calculates where the player will be by the time the rocket reaches them
    Vector3 PredictTargetPosition(Vector3 targetPos, Vector3 targetVelocity, Vector3 shooterPos, float projSpeed)
    {
        Vector3 displacement = targetPos - shooterPos;
        
        // Solving a quadratic equation to find the time of impact (t)
        // (projSpeed^2 - targetSpeed^2) * t^2 - 2 * dot(displacement, targetVelocity) * t - displacement^2 = 0
        float a = (projSpeed * projSpeed) - targetVelocity.sqrMagnitude;
        float b = -2f * Vector3.Dot(displacement, targetVelocity);
        float c = -displacement.sqrMagnitude;

        if (a == 0f) return targetPos;

        float discriminant = (b * b) - (4f * a * c);
        if (discriminant < 0f) return targetPos; // No valid intercept (e.g. target is outrunning the projectile)

        float sqrtD = Mathf.Sqrt(discriminant);
        float t1 = (-b + sqrtD) / (2f * a);
        float t2 = (-b - sqrtD) / (2f * a);

        // Find the smallest positive time
        float t = -1f;
        if (t1 > 0f && t2 > 0f) t = Mathf.Min(t1, t2);
        else if (t1 > 0f) t = t1;
        else if (t2 > 0f) t = t2;

        if (t > 0f)
        {
            // 1. Linear predicted position: current position + (velocity * time)
            Vector3 interceptPos = targetPos + (targetVelocity * t);
            
            // 2. Gravity compensation
            // Physics.gravity in Unity is typically (0, -9.81, 0). 
            // In time 't', a rigidbody falls exactly 0.5 * gravity * t^2.
            // So we need to aim *higher* by exactly that much to cancel it out!
            Vector3 gravityCompensation = -0.5f * Physics.gravity * (t * t);
            
            return interceptPos + gravityCompensation;
        }

        return targetPos; // Fallback to current position
    }

    void Update()
    {
        // 1. Handle Movement
        timeUntilWander -= Time.deltaTime;
        if (timeUntilWander < 0.0f)
        {
            timeUntilWander = 4.0f;
            var rand = Random.insideUnitCircle;
            agent.destination = transform.position + new Vector3(rand.x, 0.0f, rand.y) * 10;
        }

        // 2. Handle Aiming
        if (playerTarget != null && playerRigidbody != null)
        {
            // Point the tower at the PREDICTED player position
            Vector3 predictedPos = PredictTargetPosition(playerTarget.position, playerRigidbody.linearVelocity, tower.position, rocketSpeed);
            Vector3 directionToPredicted = predictedPos - tower.position;
            
            // Aim towards the calculated intercept point instead of the current position
            tower.rotation = Quaternion.LookRotation(directionToPredicted);
            
            // 3. Handle Shooting
            timeUntilFire -= Time.deltaTime;
            if (timeUntilFire < 0.0f)
            {
                timeUntilFire = 3.0f; // Shoot every 3 seconds
                
                // Spawn the rocket at the tower's tip, facing the same way as the tower
                var rocketInstance = Instantiate(rocket, tower.position + tower.forward * 3.5f, tower.rotation);
                rocketInstance.GetComponent<Rigidbody>().linearVelocity = tower.forward * rocketSpeed;
                
                // Tell the rocket who shot it, so the tank doesn't blow itself up
                var bombScript = rocketInstance.GetComponent<BombCollision>();
                if (bombScript != null)
                {
                    bombScript.shooterHealth = GetComponent<Health>();
                }
            }
        }
    }
}
