using UnityEngine;


public class DiscController : MonoBehaviour
{
    [SerializeField] private Teams team;
    [SerializeField] private float maxForce = 300f;
    [SerializeField] private float fixedYPosition = 0.15f;

    private Rigidbody rb;

    public Teams Team => team;
    public Rigidbody Rigidbody => rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 direction, float force)
    {
        if (direction.sqrMagnitude <= 0.0001f)
            return;

        //Acoramos la fuerza
        float clampedForce = Mathf.Clamp(force, 0f, maxForce);

        rb.Sleep();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, fixedYPosition, pos.z);

        Vector3 euler = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0f, euler.y, 0f);

        rb.WakeUp();

        Vector3 appliedForce = direction.normalized * clampedForce;
        rb.AddForce(appliedForce, ForceMode.VelocityChange);
    }
    
    public bool IsMoving(float linearThreshold = 0.05f, float angularThreshold = 0.05f)
    {
        return rb.linearVelocity.magnitude > linearThreshold ||
               rb.angularVelocity.magnitude > angularThreshold;
    }

    //Si no se frena se fuerza que se pare. Sino no puede seguir el juego
    public void ForceStop()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
    }
}