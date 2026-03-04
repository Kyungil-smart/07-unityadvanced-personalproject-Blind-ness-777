using UnityEngine;

public class EnemyBrain : MonoBehaviour, IEnemyBlackboardWriter
{
    [Header("BT Output (결정 결과)")]
    [SerializeField] private Vector3 desiredDirection = Vector3.forward;
    [SerializeField] private bool fireRequested;

    [Header("BT Input (센서 결과)")]
    [SerializeField] private bool isForwardClear = true;
    [SerializeField] private Transform blockedTarget;
    [SerializeField] private bool canAttack;
    [SerializeField] private float requestFireCooldown = 0.3f;
    private float nextRequestFireTime;

    [Header("Targets")]
    [SerializeField] private Transform baseTarget;
    [SerializeField] private Transform target; // (플레이어 등) 필요하면 센서에서 SetTarget로 받을 수 있음

    [Header("Progress / Difficulty")]
    [SerializeField] private int stageLevel = 1;
    [SerializeField] private int wallStuckCounter;

    private void Awake()
    {
        // 방향 0 방지
        if (desiredDirection == Vector3.zero) desiredDirection = Vector3.forward;
    }

    // ====== Controller가 읽는 출력 ======
    public Vector3 GetMoveDir()
    {
        // 0이면 절대 멈추지 않게 fallback
        if (desiredDirection == Vector3.zero) return transform.forward == Vector3.zero ? Vector3.forward : transform.forward;
        return desiredDirection;
    }

    public void SetDesiredDirection(Vector3 dir)
    {
        dir.y = 0f;
        if (dir == Vector3.zero) return;
        desiredDirection = SnapToCardinal(dir);
    }

    public bool ConsumeFireRequested()
    {
        if (!fireRequested) return false;
        fireRequested = false;
        return true;
    }

    public void RequestFire()
    {
        Debug.Log($"[Brain] RequestFire CALLED t={Time.time:0.00} next={nextRequestFireTime:0.00}");

        if (Time.time < nextRequestFireTime)
        {
            Debug.Log("[Brain] RequestFire BLOCKED by cooldown");
            return;
        }

        nextRequestFireTime = Time.time + requestFireCooldown;
        fireRequested = true;

        Debug.Log($"[Brain] RequestFire ACCEPTED next={nextRequestFireTime:0.00}");
    }

    public bool GetIsForwardClear()
    {
        return isForwardClear;
    }

    public Transform GetBaseTarget()
    {
        return baseTarget;
    }

    public int GetStageLevel()
    {
        return stageLevel;
    }

    public int GetWallStuckCounter()
    {
        return wallStuckCounter;
    }

    public void IncrementWallStuckCounter()
    {
        wallStuckCounter++;
    }

    public void ResetWallStuckCounter()
    {
        wallStuckCounter = 0;
    }

    // ====== EnemySensors가 쓰는 입력 인터페이스 ======
    public void SetTarget(Transform t) { target = t; }
    public void SetAttackRange(float r) { /* 필요 시 저장 */ }
    public bool GetCanAttack() { return canAttack; }
    public void SetCanAttack(bool c) { canAttack = c; }

    public void SetIsForwardClear(bool nextIsForwardClear)
    {
        isForwardClear = nextIsForwardClear;
        // Debug.Log($"[Brain] isForwardClear <= {nextIsForwardClear}");
    }

    public void SetBlockedTarget(Transform t)
    {
        blockedTarget = t;
    }

    // ====== 유틸 ======
    private Vector3 SnapToCardinal(Vector3 dir)
    {
        dir.y = 0f;
        float absX = Mathf.Abs(dir.x);
        float absZ = Mathf.Abs(dir.z);

        if (absX >= absZ)
            return (dir.x >= 0f) ? Vector3.right : Vector3.left;
        else
            return (dir.z >= 0f) ? Vector3.forward : Vector3.back;
    }
}