using UnityEngine;
using Unity.Behavior;
using Unity.Behavior.GraphFramework;

public class EnemyBrain : MonoBehaviour, IEnemyBlackboardWriter
{
    [Header("References")]
    [SerializeField] private EnemySensors sensors;
    [SerializeField] private BehaviorGraphAgent behaviorAgent;

    [Header("Blackboard (Debug)")]
    [SerializeField] private Transform target;
    [SerializeField] private float attackRange;
    [SerializeField] private bool canAttack;
    [SerializeField] private bool isForwardClear;
    [SerializeField] private Transform blockedTarget;

    [Header("Decision Output (Debug)")]
    [SerializeField] private Vector3 desiredDirection;
    [SerializeField] private bool fireRequested;

    // BT Blackboard 캐시
    private SerializableGUID desiredDirectionId;
    private SerializableGUID fireRequestedId;
    private bool hasDesiredDirectionId;
    private bool hasFireRequestedId;

    private void Awake()
    {
        if (sensors == null) sensors = GetComponent<EnemySensors>();
        if (sensors != null) sensors.SetBlackboardWriter(this);

        if (behaviorAgent == null) behaviorAgent = GetComponent<BehaviorGraphAgent>();

        // 문자열 접근은 “여기 한 번만” 쓰고, 이후엔 GUID로 접근 (오타 리스크/의존성 최소화)
        // BehaviorGraphAgent는 변수명을 통해 ID를 얻고, ID로 Get/Set이 가능함 :contentReference[oaicite:1]{index=1}
        if (behaviorAgent != null)
        {
            hasDesiredDirectionId = behaviorAgent.GetVariableID("desiredDirection", out desiredDirectionId);
            hasFireRequestedId = behaviorAgent.GetVariableID("fireRequested", out fireRequestedId);
        }

        desiredDirection = Vector3.zero;
        fireRequested = false;
    }

    // BT는 Behavior Agent가 이미 실행 중이므로, 여기서 출력값을 덮어쓰지 않는다.
    public void Tick() { }

    public Vector3 GetMoveDir()
    {
        // BT Blackboard에서 읽기
        if (behaviorAgent != null && hasDesiredDirectionId)
        {
            if (behaviorAgent.GetVariable<Vector3>(desiredDirectionId, out BlackboardVariable<Vector3> var))
            {
                desiredDirection = var.Value; // Debug 표시용
                return desiredDirection;
            }
        }

        // fallback (BT 변수 못 찾았을 때)
        return desiredDirection;
    }

    public bool ConsumeFireRequested()
    {
        if (behaviorAgent != null && hasFireRequestedId)
        {
            if (behaviorAgent.GetVariable<bool>(fireRequestedId, out BlackboardVariable<bool> var))
            {
                fireRequested = var.Value; // Debug 표시용

                if (!fireRequested) return false;

                // 소비 처리: 다시 false로 내림 (BT/코드 간 인터페이스 안정화)
                behaviorAgent.SetVariableValue<bool>(fireRequestedId, false); // :contentReference[oaicite:2]{index=2}
                fireRequested = false;
                return true;
            }
        }

        // fallback
        if (!fireRequested) return false;
        fireRequested = false;
        return true;
    }

    // ===== IEnemyBlackboardWriter (Sensors -> Brain) =====
    public void SetTarget(Transform nextTarget) => target = nextTarget;
    public void SetAttackRange(float nextAttackRange) => attackRange = nextAttackRange;
    public void SetCanAttack(bool nextCanAttack) => canAttack = nextCanAttack;
    public void SetIsForwardClear(bool nextIsForwardClear) => isForwardClear = nextIsForwardClear;
    public void SetBlockedTarget(Transform nextBlockedTarget) => blockedTarget = nextBlockedTarget;

    // ===== (선택) BT 노드가 Brain을 직접 건드리는 구조도 계속 지원 가능 =====
    public void SetDesiredDirection(Vector3 dir) => desiredDirection = dir;
    public void RequestFire() => fireRequested = true;
}