using System;
using UnityEngine;

public class DiscController : MonoBehaviour
{

    [SerializeField] private Teams team;
    [SerializeField] private float maxForce = 15f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 direction, float force)
    {
        if (direction.sqrMagnitude <= 0.0001f)
            return;
        
        //Acotar la fuerza
        float clampedForce = Mathf.Clamp(force, 0f, maxForce);
        
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        
        //No es una fuerza constante, se le da un impulso
        rb.AddForce(direction.normalized * clampedForce, ForceMode.Impulse);
    }
    
    //comprobamos si la chapa se mueve o no. Vendrá bien para el cambio de turno.
    public bool IsMoving(float threshold = 0.05f)
    {
        return rb.linearVelocity.magnitude > threshold || rb.angularVelocity.magnitude > threshold;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
