using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;

    public Rigidbody Rigidbody => rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Puede estar botando, luego comprobamos tmb componente vertical
    public bool IsReallyStopped(float linearThreshold = 0.05f, float angularThreshold = 0.05f, float verticalThreshold = 0.02f)
    {
        bool lowLinear = rb.linearVelocity.magnitude <= linearThreshold;
        bool lowAngular = rb.angularVelocity.magnitude <= angularThreshold;
        bool lowVertical = Mathf.Abs(rb.linearVelocity.y) <= verticalThreshold;

        return lowLinear && lowAngular && lowVertical;
    }

    public void ForceStop()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
    }

    public void ResetBall(Vector3 position)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = position;
        rb.Sleep();
    }
}