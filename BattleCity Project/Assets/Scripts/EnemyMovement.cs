using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float sweep = 0.2f;

    private Vector3 direction;
    private Vector3 lookDir;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // 탑뷰 환경에서 Y축 이동 및 회전 고정
        rb.constraints = RigidbodyConstraints.FreezePositionY
                         | RigidbodyConstraints.FreezeRotationX
                         | RigidbodyConstraints.FreezeRotationZ;

        // 초기 방향 고정. 미설정 시 첫 프레임 회전이 어색해짐
        lookDir = SnapToCardinal(transform.forward);
        if (lookDir == Vector3.zero) lookDir = Vector3.forward;
        rb.MoveRotation(Quaternion.LookRotation(lookDir));
    }

    // Brain에서 받은 방향을 저장. 실제 물리 적용은 FixedUpdate에서
    public void Tick(Vector3 moveDir)
    {
        direction = moveDir == Vector3.zero ? Vector3.zero : SnapToCardinal(moveDir);
    }

    private void FixedUpdate()
    {
        if (direction == Vector3.zero) return;

        rb.MoveRotation(Quaternion.LookRotation(direction));

        // 벽 감지. 충돌 예상 시 이동 중단
        if (rb.SweepTest(direction, out _, speed * Time.fixedDeltaTime + sweep))
            return;

        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private Vector3 SnapToCardinal(Vector3 dir)
    {
        if (dir == Vector3.zero) return Vector3.zero;

        dir.y = 0f;
        float absX = Mathf.Abs(dir.x);
        float absZ = Mathf.Abs(dir.z);

        return absX >= absZ
            ? (dir.x >= 0f ? Vector3.right : Vector3.left)
            : (dir.z >= 0f ? Vector3.forward : Vector3.back);
    }
}