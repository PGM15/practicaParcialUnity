using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    //RayCast - sacado de un video de youtube
    [SerializeField] private LayerMask discLayerMask;

    
    [SerializeField] private float maxDragDistance = 20f;
    [SerializeField] private float forceMultiplier = 25f;

    private DiscController selectedDisc;
    private Vector3 dragStartPoint;
    private Vector3 currentDragPoint;
    private bool isDragging = false;

    private Plane fieldPlane;

    private void Awake()
    {
        fieldPlane = new Plane(Vector3.up, Vector3.zero);

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    private void Update()
    {
        HandleSelectionAndShot();
    }

    private void HandleSelectionAndShot()
    {
        if (Input.GetMouseButtonDown(0))
            TrySelectDisc();

        if (isDragging && Input.GetMouseButton(0))
            UpdateDragPoint();

        if (isDragging && Input.GetMouseButtonUp(0))
            ShootSelectedDisc();
    }

    private void TrySelectDisc()
    {
        if (GameManager.Instance == null)
            return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, discLayerMask))
        {
            DiscController disc = hit.collider.GetComponent<DiscController>();

            if (disc != null && GameManager.Instance.CanShootDisc(disc))
            {
                selectedDisc = disc;

                if (TryGetMousePointOnField(out Vector3 point))
                {
                    dragStartPoint = point;
                    currentDragPoint = point;
                    isDragging = true;
                }
            }
        }
    }

    private void UpdateDragPoint()
    {
        if (TryGetMousePointOnField(out Vector3 point))
            currentDragPoint = point;
    }

    private void ShootSelectedDisc()
    {
        if (selectedDisc == null)
        {
            isDragging = false;
            return;
        }

        Vector3 dragVector = currentDragPoint - dragStartPoint;
        dragVector.y = 0f;

        float dragDistance = Mathf.Clamp(dragVector.magnitude, 0f, maxDragDistance);
        float shotForce = dragDistance * forceMultiplier;
        Vector3 shotDirection = -dragVector.normalized;

        if (dragDistance > 0.01f)
        {
            selectedDisc.Shoot(shotDirection, shotForce);

            if (GameManager.Instance != null)
                GameManager.Instance.NotifyShotStarted();
        }

        selectedDisc = null;
        isDragging = false;
    }

    private bool TryGetMousePointOnField(out Vector3 point)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (fieldPlane.Raycast(ray, out float enter))
        {
            point = ray.GetPoint(enter);
            return true;
        }

        point = Vector3.zero;
        return false;
    }
}