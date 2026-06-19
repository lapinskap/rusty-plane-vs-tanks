using UnityEngine;

public class FinishLine : MonoBehaviour
{
    // Unity automatically calls this when another collider enters this object's trigger zone
    void OnTriggerEnter(Collider other)
    {
        // Check if the thing entering the base is the Biplane
        if (other.GetComponentInParent<Biplane>() != null)
        {
            // Tell the manager we arrived!
            MissionManager.Instance.ReachFinishLine();
        }
    }
}
