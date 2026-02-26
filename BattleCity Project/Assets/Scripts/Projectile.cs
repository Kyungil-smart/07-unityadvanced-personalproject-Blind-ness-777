using UnityEngine;
using UnityEngine.EventSystems;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider selfCollider;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float castRadius = 0.2f;
    
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
        
        RaycastHit hit;
        if (Physics.SphereCast(prevPos, castRadius, moveDirection, out hit, distance, hitMask, QueryTriggerInteraction.Ignore))
        {
            // 자기 자신이 잡히는 경우 방지(원점이 콜라이더 안쪽이면 가끔 발생)
            if (hit.collider == selfCollider)
            {
                rb.MovePosition(prevPos + moveDirection * distance);
                return;
            }
            
            Vector3 stopPos = hit.point - moveDirection * skin;
            rb.MovePosition(stopPos);

            HandleHit(hit.collider);
            return;
        }
        
        Vector3 nextPos = prevPos + moveDirection * distance;
        rb.MovePosition(nextPos);
    }

    private void HandleHit(Collider other)
    {
        // 1) 총알끼리 충돌: 둘 다 무효화
        Projectile otherProjectile = other.GetComponent<Projectile>();
        if (otherProjectile != null)
        {
            otherProjectile.Deactivate();
            Deactivate();
            return;
        }
        
        // 2) 그 외(벽/탱크/기지): 맞으면 총알 사라짐
        Deactivate();

        // 다음 단계(내일): other 쪽이 IHitReceiver 같은 걸로 반응(벽 파괴/기지 게임오버 등)
    }
    
    private void Deactivate()
    {
        // 지금은 Destroy보다 SetActive(false)가 테스트가 편함(풀링으로 가도 그대로 사용 가능)
        gameObject.SetActive(false);
    }
}