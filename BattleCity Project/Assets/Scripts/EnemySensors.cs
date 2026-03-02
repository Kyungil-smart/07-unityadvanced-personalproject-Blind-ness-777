using UnityEngine;

public class EnemySensors : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform selfTransform;

    [Header("Combat")]
    [SerializeField] private float attackRange = 6f;

    [Header("Forward Check (Movement Block)")]
    [SerializeField] private BoxCollider bodyCollider;
    [SerializeField] private LayerMask forwardBlockMask;    // 이동을 막는 것만 포함: Water 포함 / Forest 제외
    [SerializeField] private float forwardRayForwardExtra = 0.15f;
    [SerializeField] private float forwardCheckDistance = 0.8f;
    [SerializeField] private float forwardRayHeight = 0.2f; // 바닥에 박히는 문제 방지

    [Header("Debug Readonly")]
    [SerializeField] private bool canAttack;
    [SerializeField] private bool isForwardClear;
    [SerializeField] private Transform blockedTarget;

    // Blackboard 연결부(일단 인터페이스만)
    // 나중에 EnemyBrain에서 Blackboard 참조를 넘겨주거나, 여기서 직접 참조하게 바꿔도 됨
    private IEnemyBlackboardWriter blackboardWriter;

    private void Awake()
    {
        if (selfTransform == null) selfTransform = transform;
    }

    // EnemyController/EnemyBrain에서 1회 주입해서 사용
    public void SetBlackboardWriter(IEnemyBlackboardWriter writer)
    {
        blackboardWriter = writer;
    }

    public void Tick(Vector3 forwardDirection)
    {
        // 1) Target(플레이어) 확정
        Transform target = playerTransform;

        // 2) CanAttack 계산(거리만, LOS는 나중에 추가)
        bool nextCanAttack = false;
        if (target != null)
        {
            Vector3 diff = target.position - selfTransform.position;
            diff.y = 0f; // 탑뷰 기준 거리
            float sqrDist = diff.sqrMagnitude;
            float sqrRange = attackRange * attackRange;

            nextCanAttack = sqrDist <= sqrRange;
        }

        // 3) IsForwardClear / BlockedTarget 계산
        Transform nextBlockedTarget = null;
        bool nextIsForwardClear = true;

        if (forwardDirection != Vector3.zero)
        {
            Vector3 origin = GetForwardRayOrigin(forwardDirection);
            Vector3 dir = forwardDirection.normalized;

            RaycastHit hit;
            if (Physics.Raycast(origin, dir, out hit, forwardCheckDistance, forwardBlockMask, QueryTriggerInteraction.Ignore))
            {
                nextIsForwardClear = false;
                nextBlockedTarget = hit.transform;
            }
        }

        // 4) 내부 상태 업데이트(디버그 확인용)
        canAttack = nextCanAttack;
        isForwardClear = nextIsForwardClear;
        blockedTarget = nextBlockedTarget;

        // 5) Blackboard에 쓰기(연결되면 바로 동작)
        if (blackboardWriter != null)
        {
            blackboardWriter.SetTarget(target);
            blackboardWriter.SetAttackRange(attackRange);
            blackboardWriter.SetCanAttack(canAttack);
            blackboardWriter.SetIsForwardClear(isForwardClear);
            blackboardWriter.SetBlockedTarget(blockedTarget);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (selfTransform == null) return;

        Gizmos.matrix = Matrix4x4.identity;

        // 공격 범위
        Gizmos.DrawWireSphere(selfTransform.position, attackRange);

        // 전방 레이(디버그)
        Vector3 origin = selfTransform.position + Vector3.up * forwardRayHeight;
        Vector3 dir = selfTransform.forward;

        Gizmos.DrawLine(origin, origin + dir * forwardCheckDistance);
    }
    
    private Vector3 GetForwardRayOrigin(Vector3 forwardDir)
    {
        Vector3 dir = forwardDir.normalized;

        // 기본값(콜라이더 못 받았을 때 최소한 작동)
        float forwardOffset = 0.6f;

        if (bodyCollider != null)
        {
            // 월드 기준 반 크기
            Vector3 extents = bodyCollider.bounds.extents;
            
            // forward 방향 기준으로 가장 큰 축 선택
            float offset = Mathf.Abs(Vector3.Dot(extents, transform.forward));
            
            forwardOffset = offset + forwardRayForwardExtra;

            return bodyCollider.bounds.center
                   + Vector3.up * forwardRayHeight
                   + dir * forwardOffset;
        }

        return transform.position + Vector3.up * forwardRayHeight + dir * (forwardOffset + forwardRayForwardExtra);
    }
}

// Blackboard에 값을 쓰기 위한 최소 인터페이스
// EnemyBrain 쪽에서 BehaviorTree Blackboard에 맞춰 구현하면 됨
public interface IEnemyBlackboardWriter
{
    void SetTarget(Transform target);
    void SetAttackRange(float attackRange);
    void SetCanAttack(bool canAttack);
    void SetIsForwardClear(bool isForwardClear);
    void SetBlockedTarget(Transform blockedTarget);
}