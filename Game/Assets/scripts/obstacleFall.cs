using UnityEngine;

public class obstacleFall : MonoBehaviour
{

    public float moveSpeed = 9.0f; // Speed of the obstacle's movement
    private Vector3 defaultPos;
    private float fallTime = 1.0f; // Time to start falling
    private float returnTime = 2.0f; // Time to start returning
    private float fallTimer = 0.0f; // Timer for falling
    private float returnTimer = 0.0f; // Timer for returning
    private bool isFalling = false; // Is the obstacle currently falling?
    private bool isReturning = false; // Is the obstacle currently returning?

    void Start()
    {
        defaultPos = transform.position;
    }

    void Update()
    {
        // Handle falling
        if (isFalling)
        {
            fallTimer += Time.deltaTime;

            if (fallTimer >= fallTime)
            {
                // Move down
                transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
            }
        }

        // Handle returning
        if (isReturning)
        {
            returnTimer += Time.deltaTime;

            if (returnTimer >= returnTime)
            {
                // Move up until the obstacle is back at the default position
                if (Vector3.Distance(transform.position, defaultPos) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, defaultPos, moveSpeed * Time.deltaTime);
                }
                else
                {
                    // Reset state once back in original position
                    ResetState();
                }
            }
            else
            {
                // Continue falling
                transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            // Reset the object's state if it's returning
            if (isReturning)
            {
                ResetState();
            }

            isFalling = true;
            fallTimer = 0.0f; // Reset timer when player steps on
        }
    }



    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            isFalling = false;
            isReturning = true;
            returnTimer = 0.0f; // Reset timer when player steps off
        }
    }

    private void ResetState()
    {
        isFalling = false;
        isReturning = false;
        fallTimer = 0.0f;
        returnTimer = 0.0f;
    }
}
