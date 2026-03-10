using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public bool IsMoving(float threshold = 0.05f)
    {
        return rb.linearVelocity.magnitude > threshold || rb.angularVelocity.magnitude > threshold;
    }

    public void ResetBall(Vector3 position)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = position;
    }
}