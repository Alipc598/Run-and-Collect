using UnityEngine;

public class obstacleMoveY : MonoBehaviour
{
    public float amplitude = 16.0f;
    public float frequency = 5.0f;
    public float knockBackForce = 5.0f;

    public float rotationSpeed = 50.0f; // Degrees per second for each axis

    private Vector3 startPosition;
    private Vector3 newPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the object
        newPosition = startPosition;
        newPosition.z += Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = newPosition;

        // Spin the object
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                Vector3 direction = (collision.transform.position - transform.position).normalized;
                direction.y = 1;

                playerRigidbody.AddForce(direction.normalized * knockBackForce, ForceMode.Impulse);
            }
        }
    }
}
