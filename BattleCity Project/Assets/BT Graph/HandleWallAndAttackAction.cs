using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "HandleWallAndAttack",
    story: "Update DesiredDirection to turn right",
    category: "Action",
    id: "HANDLE_WALL_AND_ATTACK_ACTION"
)]
public partial class HandleWallAndAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;          // ✅ 추가
    [SerializeReference] public BlackboardVariable<Vector3> DesiredDirection;

    protected override Status OnStart()
    {
        if (Self == null || Self.Value == null) return Status.Failure;

        var brain = Self.Value.GetComponent<EnemyBrain>();
        if (brain == null) return Status.Failure;

        // 앞이 뚫렸으면: 벽 연속 카운트 리셋 + 회피 액션은 실행하지 않음
        if (brain.GetIsForwardClear())
        {
            brain.ResetWallStuckCounter();
            return Status.Failure;
        }

        // ====== 여기부터 "벽을 만났을 때" 확률 의사결정 ======
        brain.IncrementWallStuckCounter();

        int stageLevel = brain.GetStageLevel();
        int stuck = brain.GetWallStuckCounter();

        // 기본 공격 확률: 스테이지가 오를수록 증가
        // (수치는 지금 1차 값. 원하면 다음 단계에서 튜닝)
        float baseShoot = 0.05f + 0.03f * Mathf.Max(0, stageLevel - 1);

        // 연속 벽 보정: 벽 연속일수록 증가
        float stuckBonus = 0.05f * Mathf.Max(0, stuck - 1);

        float shootChance = Mathf.Clamp(baseShoot + stuckBonus, 0.05f, 0.75f);

        // 회전/후진 비율 (나머지 확률을 회피로 사용)
        // 뒤로(180)는 너무 자주면 이상해지니 작은 확률로
        float backChance = 0.15f; // 회피 중 15%는 180도
        float roll = UnityEngine.Random.value;

        // 1) 공격
        if (roll < shootChance)
        {
            brain.RequestFire();
            Debug.Log($"[BT] HandleWallAndAttack => FIRE (stage={stageLevel}, stuck={stuck}, p={shootChance:0.00})");
            return Status.Success;
        }

        // 2) 회피 방향 결정 (좌/우/뒤)
        Vector3 cur = DesiredDirection != null ? DesiredDirection.Value : Vector3.zero;
        if (cur == Vector3.zero) cur = brain.GetMoveDir();
        if (cur == Vector3.zero) cur = Vector3.forward;

        // 후보 3개: 오른쪽/왼쪽/뒤
        Vector3 right = new Vector3(-cur.z, 0f, cur.x);
        Vector3 left  = new Vector3(cur.z, 0f, -cur.x);
        Vector3 back  = new Vector3(-cur.x, 0f, -cur.z);

        // 기지 방향(스냅 전 벡터) 구함
        Vector3 toBase = Vector3.zero;
        Transform baseT = brain.GetBaseTarget();
        if (baseT != null)
        {
            toBase = baseT.position - Self.Value.transform.position;
            toBase.y = 0f;
        }

        // 점수: 기지 방향과 더 “같은” 방향(내적이 큰 방향)을 우선
        float scoreRight = (toBase == Vector3.zero) ? 0f : Vector3.Dot(right.normalized, toBase.normalized);
        float scoreLeft  = (toBase == Vector3.zero) ? 0f : Vector3.Dot(left.normalized,  toBase.normalized);
        float scoreBack  = (toBase == Vector3.zero) ? -1f : Vector3.Dot(back.normalized,  toBase.normalized);

        // 뒤로는 너무 자주면 답답하니까 패널티(원하면 다음 단계에서 조정)
        scoreBack -= 0.35f;

        // 최고 점수 방향 선택 (동점이면 랜덤)
        Vector3 next = right;
        float best = scoreRight;

        if (scoreLeft > best) { best = scoreLeft; next = left; }
        if (scoreBack > best) { best = scoreBack; next = back; }

        // 아주 가끔은 랜덤성 유지(“확률 기반” 느낌 유지)
        if (UnityEngine.Random.value < 0.15f)
        {
            float r = UnityEngine.Random.value;
            next = (r < 0.45f) ? right : (r < 0.9f ? left : back);
        }

        if (DesiredDirection != null) DesiredDirection.Value = next;
        brain.SetDesiredDirection(next);

        Debug.Log($"[BT] TURN bias => dir={next} scores R={scoreRight:0.00} L={scoreLeft:0.00} B={scoreBack:0.00}");
        return Status.Success;
    }

    protected override Status OnUpdate() => Status.Success;
    protected override void OnEnd() { }
}