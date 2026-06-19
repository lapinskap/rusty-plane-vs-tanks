using UnityEngine;

public class CheckpointRing : MonoBehaviour
{
    private bool isCollected = false;

    // Unity automatically calls this when another collider enters this object's trigger zone
    void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;

        // Check if the thing flying through us is the Biplane
        if (other.GetComponentInParent<Biplane>() != null)
        {
            isCollected = true;
            
            // Tell the manager we got one!
            MissionManager.Instance.CollectRing();

            // Hide the ring (or you could play a fancy particle effect here)
            gameObject.SetActive(false); 
        }
    }
}
