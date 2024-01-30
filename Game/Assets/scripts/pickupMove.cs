using UnityEngine;

public class pickupMove : MonoBehaviour
{
    public Vector3 spinSpeed = new Vector3(0f, 0f, 180f); // Speed of spin in degrees per second around each axis.

    // Update is called once per frame
    void Update()
    {
        // Handle the spinning effect.
        transform.Rotate(spinSpeed * Time.deltaTime);
    }
}
