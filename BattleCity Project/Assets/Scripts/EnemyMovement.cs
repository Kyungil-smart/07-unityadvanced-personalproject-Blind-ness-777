using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 3f;

    private Vector3 direction;
    private Vector3 lookDir;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        
        rb.useGravity = false;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void Tick(Vector3 moveDir)
    {
        direction = moveDir;

        if (direction != Vector3.zero)
        {
            lookDir = direction;
            rb.MoveRotation(Quaternion.LookRotation(lookDir));
        }
    }

    private void FixedUpdate()
    {
        if (direction == Vector3.zero) return;
        
        Vector3 currentPos = rb.position;
        float distance = speed * Time.fixedDeltaTime;

        Vector3 nextPos = currentPos + direction * distance;
        rb.MovePosition(nextPos);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HIT: " + collision.gameObject.name);
    }
}
