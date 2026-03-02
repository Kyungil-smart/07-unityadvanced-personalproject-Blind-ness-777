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

        // 탑뷰 탱크 안정화(필요하면 조절)
        rb.constraints = RigidbodyConstraints.FreezePositionY
                         | RigidbodyConstraints.FreezeRotationX
                         | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Tick(Vector3 moveDir)
    {
        // 1) 의도 저장 (물리 적용은 FixedUpdate에서)
        if (moveDir == Vector3.zero)
        {
            direction = Vector3.zero;
            return;
        }

        direction = moveDir.normalized;
        lookDir = direction;
    }

    private void FixedUpdate()
    {
        if (direction == Vector3.zero) return;

        // 2) 회전도 물리 스텝에서 적용
        rb.MoveRotation(Quaternion.LookRotation(lookDir));

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