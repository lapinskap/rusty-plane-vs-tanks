using UnityEngine;
using UnityEngine.InputSystem;

public class Biplane : MonoBehaviour
{
    public GameObject missile;
    
    public Transform bombDropPoint;

    InputAction moveAction;
    InputAction moveFasterAction;
    InputAction fireMissileAction;
    Rigidbody rigidBody;
    Transform propeller;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveFasterAction = InputSystem.actions.FindAction("Sprint");
        rigidBody = GetComponent<Rigidbody>();
        propeller = transform.Find("Propeller");

        fireMissileAction = InputSystem.actions.FindAction("FireMissile");
        fireMissileAction.performed += ctx => FireMissile();

        // Listen for our own death to handle camera detachment and explosion!
        Health myHealth = GetComponent<Health>();
        if (myHealth != null)
        {
            myHealth.OnDeath.AddListener(OnBiplaneDestroyed);
        }
    }

    void OnBiplaneDestroyed()
    {
        // 1. Detach the Main Camera so it survives the plane's destruction
        Camera mainCam = Camera.main;
        if (mainCam != null && mainCam.transform.IsChildOf(transform))
        {
            mainCam.transform.SetParent(null);
        }

        // 2. Create a massive explosion using the effect we already built!
        GameObject bigExplosion = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bigExplosion.transform.position = transform.position;
        Destroy(bigExplosion.GetComponent<Collider>()); // No physics needed
        
        ExplosionEffect effect = bigExplosion.AddComponent<ExplosionEffect>();
        // We can't easily change the lifetime since it's hardcoded in the script right now, 
        // but it will look like a nice big blast!
    }

    void FireMissile()
    {
        // 1. Determine spawn position. Use the assigned point, or default to 1.5 units below the plane's center.
        Vector3 spawnPos = bombDropPoint != null ? bombDropPoint.position : transform.position - (transform.up * 1.5f);

        var missileInstance = Instantiate(missile, spawnPos, transform.rotation);
        
        // 2. Realistic Bomb Physics
        // A dropped bomb should inherit the plane's current momentum, plus a little downward push
        Vector3 inheritedVelocity = rigidBody.linearVelocity;
        Vector3 downwardPush = -transform.up * 1.0f; // Push it 'down' relative to the plane's rotation
        
        missileInstance.GetComponent<Rigidbody>().linearVelocity = inheritedVelocity + downwardPush;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVec = moveAction.ReadValue<Vector2>();
        bool isMovingFaster = moveFasterAction.ReadValue<float>() > 0.5f;
        float speedMultiplier = isMovingFaster ? 2.0f : 1.0f;

        rigidBody.AddRelativeTorque(new Vector3(-moveVec.x, 0.0f, -moveVec.y) * speedMultiplier);
        rigidBody.AddForce(transform.right * speedMultiplier);
        propeller.rotation *= Quaternion.Euler(1500.0f * Time.deltaTime, 0.0f, 0.0f);
    }
}
