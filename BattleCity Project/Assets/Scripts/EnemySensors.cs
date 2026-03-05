using UnityEngine;

public class EnemySensors : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform selfTransform;

    [Header("Forward Check")]
    [SerializeField] private BoxCollider bodyCollider;
    [SerializeField] private LayerMask forwardBlockMask;
    [SerializeField] private float forwardRayForwardExtra = 0.15f;
    [SerializeField] private float forwardCheckDistance = 0.8f;
    [SerializeField] private float forwardRayHeight = 0.2f;

    [Header("Debug Readonly")]
    [SerializeField] private bool isForwardClear;
    [SerializeField] private Transform blockedTarget;

    private IEnemyBlackboardWriter blackboardWriter;

    private void Awake()
    {
        if (selfTransform == null) selfTransform = transform;
    }

    public void SetBlackboardWriter(IEnemyBlackboardWriter writer)
    {
        blackboardWriter = writer;
    }

    // 전방 레이캐스트로 장애물 감지 후 Brain에 결과 전달
    public void Tick(Vector3 forwardDirection)
    {
        Transform nextBlockedTarget = null;
        bool nextIsForwardClear = true;

        if (forwardDirection != Vector3.zero)
        {
            Vector3 origin = GetForwardRayOrigin(forwardDirection);
            Vector3 dir = forwardDirection.normalized;

            if (Physics.Raycast(origin, dir, out RaycastHit hit, forwardCheckDistance, forwardBlockMask, QueryTriggerInteraction.Ignore))
            {
                nextIsForwardClear = false;
                nextBlockedTarget = hit.transform;
            }
        }

        isForwardClear = nextIsForwardClear;
        blockedTarget = nextBlockedTarget;

        if (blackboardWriter != null)
        {
            blackboardWriter.SetTarget(playerTransform);
            blackboardWriter.SetAttackRange(0f);
            blackboardWriter.SetIsForwardClear(isForwardClear);
            blackboardWriter.SetBlockedTarget(blockedTarget);
        }
    }

    // 콜라이더 크기 기반으로 레이 시작점 계산
    private Vector3 GetForwardRayOrigin(Vector3 forwardDir)
    {
        Vector3 dir = forwardDir.normalized;
        Vector3 center = bodyCollider != null ? bodyCollider.bounds.center : transform.position;
        float forwardOffset = 0.6f;

        if (bodyCollider != null)
        {
            float ext = Mathf.Max(bodyCollider.bounds.extents.x, bodyCollider.bounds.extents.z);
            forwardOffset = ext + forwardRayForwardExtra;
        }

        return center + Vector3.up * forwardRayHeight + dir * forwardOffset;
    }

    private void OnDrawGizmosSelected()
    {
        if (selfTransform == null) return;

        Gizmos.DrawWireSphere(selfTransform.position, 0.3f);

        Vector3 origin = selfTransform.position + Vector3.up * forwardRayHeight;
        Gizmos.DrawLine(origin, origin + selfTransform.forward * forwardCheckDistance);
    }
}