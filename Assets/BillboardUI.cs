using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        // Try to find the Main Camera by tag, fallback to just finding ANY camera if the tag is missing
        mainCam = Camera.main;
        if (mainCam == null)
        {
            mainCam = Object.FindAnyObjectByType<Camera>();
        }
    }

    void LateUpdate()
    {
        if (mainCam != null)
        {
            // The safest and most foolproof way to billboard UI in Unity:
            // Just copy the exact rotation of the camera
            transform.rotation = mainCam.transform.rotation;
        }
    }
}