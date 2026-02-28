using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider selfCollider;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float castRadius = 0.2f;

    [Header("Cast Offset")]
    [SerializeField] private float castYOffset = 0.3f; // 판정용 캐스트를 아래로 내리는 값(월드 Y 기준)

    private float moveSpeed;
    private Vector3 moveDirection;

    private readonly float skin = 0.01f;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (selfCollider == null) selfCollider = GetComponent<Collider>();

        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void Launch(Vector3 direction, float speed)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
    }

    private void FixedUpdate()
    {
        if (moveSpeed <= 0f) return;
        if (moveDirection == Vector3.zero) return;

        Vector3 prevPos = rb.position;
        float distance = moveSpeed * Time.fixedDeltaTime;

        // ✅ 캐스트 시작점만 아래로 내림
        Vector3 castOffset = Vector3.down * castYOffset;
        Vector3 castOrigin = prevPos + castOffset;

        RaycastHit hit;
        if (Physics.SphereCast(castOrigin, castRadius, moveDirection, out hit, distance, hitMask, QueryTriggerInteraction.Ignore))
        {
            // 자기 자신이 잡히는 경우 방지
            if (hit.collider == selfCollider)
            {
                rb.MovePosition(prevPos + moveDirection * distance);
                return;
            }

            // ✅ hit.point는 "캐스트 중심" 기준이므로, 다시 오프셋을 되돌려서 실제 총알 위치를 계산
            Vector3 stopPos = (hit.point - moveDirection * skin) - castOffset;
            rb.MovePosition(stopPos);

            HandleHit(hit.collider, hit);
            return;
        }

        Vector3 nextPos = prevPos + moveDirection * distance;
        rb.MovePosition(nextPos);
    }

    private void HandleHit(Collider other, RaycastHit hit)
    {
        Projectile otherProjectile = other.GetComponent<Projectile>();
        if (otherProjectile != null)
        {
            otherProjectile.Deactivate();
            Deactivate();
            return;
        }

        IProjectileHittable hittable = other.GetComponent<IProjectileHittable>();
        if (hittable != null)
        {
            hittable.OnProjectileHit(this, hit);
        }

        Deactivate();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}