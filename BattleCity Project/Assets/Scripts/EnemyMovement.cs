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
        
        // 초기 바라보는 방향 고정 (없으면 첫 턴에서 이상해질 수 있음)
        lookDir = SnapToCardinal(transform.forward);
        if (lookDir == Vector3.zero) lookDir = Vector3.forward;
        rb.MoveRotation(Quaternion.LookRotation(lookDir));
    }

    public void Tick(Vector3 moveDir)
    {
        // 의도 저장 (물리 적용은 FixedUpdate에서)
        if (moveDir == Vector3.zero)
        {
            direction = Vector3.zero;
            return;
        }

        direction = SnapToCardinal(moveDir);
    }

    private void FixedUpdate()
    {
        if (direction == Vector3.zero) return;

        // 방향이 바뀌었으면 회전만 하고 종료 (턴 프레임)
        if (direction != lookDir)
        {
            lookDir = direction;
            rb.MoveRotation(Quaternion.LookRotation(lookDir));
            return;
        }
        
        // 방향 같으면 직선 이동
        Vector3 currentPos = rb.position;
        float distance = speed * Time.fixedDeltaTime;

        Vector3 nextPos = currentPos + lookDir * distance;
        rb.MovePosition(nextPos);
    }
    
    private Vector3 SnapToCardinal(Vector3 dir)
    {
        if (dir == Vector3.zero) return Vector3.zero;

        dir.y = 0f;

        float absX = Mathf.Abs(dir.x);
        float absZ = Mathf.Abs(dir.z);

        if (absX >= absZ)
            return (dir.x >= 0f) ? Vector3.right : Vector3.left;
        else
            return (dir.z >= 0f) ? Vector3.forward : Vector3.back;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HIT: " + collision.gameObject.name);
    }
}