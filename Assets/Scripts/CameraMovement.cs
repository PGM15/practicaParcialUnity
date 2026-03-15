using UnityEngine;


// Gestiona tener una camara libre
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float mouseSensitivity = 20f;

    private float hInput, vInput;
    private float pitch;
    private float yaw;

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        pitch = angles.x;
        yaw = angles.y;
    }

    void Update()
    {
        Move();
        RotateCamera();
    }

    void Move()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        Vector3 direction = hInput * transform.right + vInput * transform.forward;
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    void RotateCamera()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 mouse = Input.mousePositionDelta;

            yaw += mouse.x * mouseSensitivity * Time.deltaTime;
            pitch -= mouse.y * mouseSensitivity * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, -89f, 89f);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}