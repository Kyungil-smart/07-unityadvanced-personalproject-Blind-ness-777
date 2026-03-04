using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "SetTargetDirection",
    story: "Update DesiredDirection toward base",
    category: "Action",
    id: "SET_TARGET_DIRECTION_ACTION"
)]
public partial class SetTargetDirectionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Vector3> DesiredDirection;
    [SerializeReference] public BlackboardVariable<Transform> BaseTarget;

    protected override Status OnStart()
    {
        // Self 확인
        if (Self == null || Self.Value == null) return Status.Failure;

        var brain = Self.Value.GetComponent<EnemyBrain>();
        if (brain == null) return Status.Failure;

        // BaseTarget 자동 탐색 (Layer = Base, 씬에 1개)
        if (BaseTarget != null && BaseTarget.Value == null)
        {
            int baseLayer = LayerMask.NameToLayer("Base");
            if (baseLayer >= 0)
            {
                GameObject[] all = UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
                for (int i = 0; i < all.Length; i++)
                {
                    if (all[i].layer == baseLayer)
                    {
                        BaseTarget.Value = all[i].transform;
                        break;
                    }
                }
            }
        }

        if (BaseTarget == null || BaseTarget.Value == null)
            return Status.Success; // 베이스 없으면 유지

        // 방향 계산
        Vector3 dir = BaseTarget.Value.position - Self.Value.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
            return Status.Success;

        dir.Normalize();

        // 4방향 스냅
        Vector3 snapDir = Mathf.Abs(dir.x) >= Mathf.Abs(dir.z)
            ? (dir.x >= 0f ? Vector3.right : Vector3.left)
            : (dir.z >= 0f ? Vector3.forward : Vector3.back);

        // Blackboard + Brain 모두 반영 (컨트롤러는 Brain만 읽어도 됨)
        if (DesiredDirection != null) DesiredDirection.Value = snapDir;
        brain.SetDesiredDirection(snapDir);
        
        // 벽이 없어도 공격: "사거리 안"일 때만 확률 발사
        if (brain.GetCanAttack())
        {
            int stageLevel = brain.GetStageLevel();

            // 스테이지가 오를수록 기본 발사 확률 증가 (수치는 1차 튜닝값)
            float shootChance = Mathf.Clamp(0.02f + 0.02f * Mathf.Max(0, stageLevel - 1), 0.02f, 0.25f);

            if (UnityEngine.Random.value < shootChance)
            {
                brain.RequestFire();
                Debug.Log($"[BT] MoveFire => FIRE (stage={stageLevel}, p={shootChance:0.00})");
            }
        }

        // 벽 카운트 리셋(앞이 뚫리면 다시 “기지로”가 우선)
        brain.ResetWallStuckCounter();

        Debug.Log($"[BT] SetTargetDirection => {snapDir}");
        return Status.Success;
    }

    protected override Status OnUpdate() => Status.Success;
    protected override void OnEnd() { }
}