using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider selfCollider;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float castRadius = 0.2f;

    [Header("Cast Offset")]
    [SerializeField] private float castYOffset = 0.3f;

    private float moveSpeed;
    private Vector3 moveDirection;
    private int ownerLayer;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (selfCollider == null) selfCollider = GetComponent<Collider>();

        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    // 발사 방향, 속도, 발사자 레이어 설정. 같은 레이어는 피격 무시
    public void Launch(Vector3 direction, float speed, int layer = -1)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
        ownerLayer = layer;
        if (rb != null) rb.position = transform.position;
    }

    private void FixedUpdate()
    {
        if (moveSpeed <= 0f || moveDirection == Vector3.zero) return;

        Vector3 prevPos = rb.position;
        float distance = moveSpeed * Time.fixedDeltaTime;

        // 판정 캐스트를 아래로 내려 지형 충돌 오감지 방지
        Vector3 castOffset = Vector3.down * castYOffset;
        Vector3 castOrigin = prevPos + castOffset;

        if (Physics.SphereCast(castOrigin, castRadius, moveDirection, out RaycastHit hit, distance, hitMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider == selfCollider)
            {
                rb.MovePosition(prevPos + moveDirection * distance);
                return;
            }

            // 캐스트 오프셋 되돌려서 실제 총알 정지 위치 계산
            Vector3 stopPos = prevPos + moveDirection * hit.distance - castOffset;
            rb.MovePosition(stopPos);

            HandleHit(hit.collider, hit);
            return;
        }

        rb.MovePosition(prevPos + moveDirection * distance);
    }

    private void OnDisable()
    {
        moveSpeed = 0f;
        moveDirection = Vector3.zero;
    }

    // 발사자 레이어 무시, 총알끼리 상쇄, IProjectileHittable 피격 처리
    private void HandleHit(Collider other, RaycastHit hit)
    {
        if (ownerLayer != -1 && other.gameObject.layer == ownerLayer)
            return;

        Projectile otherProjectile = other.GetComponent<Projectile>();
        if (otherProjectile != null)
        {
            otherProjectile.Deactivate();
            Deactivate();
            return;
        }

        IProjectileHittable hittable = other.GetComponent<IProjectileHittable>();
        if (hittable != null)
            hittable.OnProjectileHit(this, hit);

        Deactivate();
    }

    private void Deactivate()
    {
        moveSpeed = 0f;
        moveDirection = Vector3.zero;
        gameObject.SetActive(false);
    }
}