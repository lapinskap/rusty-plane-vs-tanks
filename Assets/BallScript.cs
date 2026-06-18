using UnityEngine;

public class BallScript : MonoBehaviour
{
    public Vector3 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //transform.position += direction * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += direction * Time.deltaTime;
    }

    // Destruct the ball after 10 seconds
    void OnEnable()
    {
        Destroy(gameObject, 10.0f);
    }
}
