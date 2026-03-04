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

        // 회전은 무조건 실행
        rb.MoveRotation(Quaternion.LookRotation(direction));

        // 이동 전 SweepTest (0.2f 여유)
        if (rb.SweepTest(direction, out _, speed * Time.fixedDeltaTime + 0.2f))
        {
            return; // 벽이면 이동만 중단
        }

        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
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
        // Debug.Log("HIT: " + collision.gameObject.name);
    }
}