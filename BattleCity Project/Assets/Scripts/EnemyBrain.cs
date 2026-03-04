using UnityEngine;

public class EnemyBrain : MonoBehaviour, IEnemyBlackboardWriter
{
    [Header("결정 결과")]
    [SerializeField] private Vector3 desiredDirection = Vector3.forward;
    [SerializeField] private bool fireRequested;

    [Header("센서 결과")]
    [SerializeField] private bool isForwardClear = true;

    [Header("Targets")]
    [SerializeField] private Transform baseTarget;

    [Header("난이도")]
    [SerializeField] private int stageLevel = 1;
    [SerializeField] private int wallStuckCounter;

    [Header("발사 주기")]
    [SerializeField] private float tryInterval = 0.25f;
    private float nextTryTime;
    
    [Header("랜덤 방향 전환")]
    [SerializeField] private float randomTurnInterval = 2f;
    private float nextRandomTurnTime;

    private void Awake()
    {
        if (desiredDirection == Vector3.zero) desiredDirection = Vector3.forward;
    }

    public void Think()
    {
        TryFire();

        if (isForwardClear)
            MoveTowardBase();
        else
            HandleWall();
    }

    private void TryFire()
    {
        if (Time.time < nextTryTime) return;
        nextTryTime = Time.time + tryInterval;

        float baseP = 0.05f + 0.03f * Mathf.Max(0, stageLevel - 1);
        float stuckP = 0.05f * Mathf.Clamp(wallStuckCounter, 0, 4);
        float p = Mathf.Clamp(baseP + stuckP, 0.05f, 0.45f);

        if (Random.value < p)
            fireRequested = true;
    }

    private void MoveTowardBase()
    {
        wallStuckCounter = 0;

        if (baseTarget == null) return;

        // 일정 확률로 랜덤 방향 전환
        if (Time.time >= nextRandomTurnTime)
        {
            nextRandomTurnTime = Time.time + randomTurnInterval;

            if (Random.value < 0.3f)
            {
                Vector3 cur = desiredDirection;
                Vector3 right = new Vector3(-cur.z, 0f, cur.x);
                Vector3 left  = new Vector3(cur.z, 0f, -cur.x);
                desiredDirection = Random.value < 0.5f ? right : left;
                return;
            }
        }

        Vector3 dir = baseTarget.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f) return;

        desiredDirection = SnapToCardinal(dir.normalized);
    }

    private void HandleWall()
    {
        wallStuckCounter++;

        float baseShoot = 0.05f + 0.03f * Mathf.Max(0, stageLevel - 1);
        float stuckBonus = 0.05f * Mathf.Max(0, wallStuckCounter - 1);
        float shootChance = Mathf.Clamp(baseShoot + stuckBonus, 0.05f, 0.45f);

        if (Random.value < shootChance)
        {
            fireRequested = true;
            return;
        }

        Vector3 cur = desiredDirection;
        Vector3 right = new Vector3(-cur.z, 0f, cur.x);
        Vector3 left  = new Vector3(cur.z, 0f, -cur.x);
        Vector3 back  = new Vector3(-cur.x, 0f, -cur.z);

        Vector3 toBase = Vector3.zero;
        if (baseTarget != null)
        {
            toBase = baseTarget.position - transform.position;
            toBase.y = 0f;
        }

        float scoreRight = toBase == Vector3.zero ? 0f : Vector3.Dot(right.normalized, toBase.normalized);
        float scoreLeft  = toBase == Vector3.zero ? 0f : Vector3.Dot(left.normalized, toBase.normalized);
        float scoreBack  = (toBase == Vector3.zero ? -1f : Vector3.Dot(back.normalized, toBase.normalized)) - 0.35f;

        Vector3 next = right;
        float best = scoreRight;
        if (scoreLeft > best) { best = scoreLeft; next = left; }
        if (scoreBack > best) { next = back; }

        if (Random.value < 0.15f)
        {
            float r = Random.value;
            next = r < 0.45f ? right : r < 0.9f ? left : back;
        }

        desiredDirection = next;
    }

    // Controller가 읽는 출력
    public Vector3 GetMoveDir()
    {
        if (desiredDirection == Vector3.zero)
            return transform.forward == Vector3.zero ? Vector3.forward : transform.forward;
        return desiredDirection;
    }

    public bool ConsumeFireRequested()
    {
        if (!fireRequested) return false;
        fireRequested = false;
        return true;
    }

    public void RequestFire() => fireRequested = true;

    public bool GetIsForwardClear() => isForwardClear;
    public Transform GetBaseTarget() => baseTarget;
    public int GetStageLevel() => stageLevel;
    public int GetWallStuckCounter() => wallStuckCounter;
    public void IncrementWallStuckCounter() => wallStuckCounter++;
    public void ResetWallStuckCounter() => wallStuckCounter = 0;

    // IEnemyBlackboardWriter
    public void SetTarget(Transform t) { }
    public void SetAttackRange(float r) { }
    public void SetIsForwardClear(bool v) => isForwardClear = v;
    public void SetBlockedTarget(Transform t) { }

    // 유틸
    private Vector3 SnapToCardinal(Vector3 dir)
    {
        dir.y = 0f;
        float absX = Mathf.Abs(dir.x);
        float absZ = Mathf.Abs(dir.z);
        return absX >= absZ
            ? (dir.x >= 0f ? Vector3.right : Vector3.left)
            : (dir.z >= 0f ? Vector3.forward : Vector3.back);
    }
}