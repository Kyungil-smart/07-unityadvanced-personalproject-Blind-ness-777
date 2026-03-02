using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ResolveBlockedDirection", story: "Resolve blocked direction -> [NewDirection]", category: "Action", id: "8f97bda05532d6ad6e52441f39126945")]
public partial class ResolveBlockedDirectionAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> NewDirection;
    [SerializeReference] public BlackboardVariable<int> StuckCount;

    protected override Status OnStart()
    {
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        if (NewDirection == null) return Status.Failure;

        // 연속 랜덤 방지: 이미 막힘 처리(회전 시도)를 한 상태면 방향을 다시 바꾸지 않는다.
        if (StuckCount != null && StuckCount.Value > 0)
        {
            return Status.Success;
        }

        Vector3 currentDir = NewDirection.Value;
        if (currentDir == Vector3.zero)
            currentDir = Vector3.forward; // 0이면 기준 방향 하나 잡아줌(막힘 상황에서 계산용)

        // 좌 / 우 / 뒤
        Vector3 left  = new Vector3(-currentDir.z, 0f, currentDir.x);
        Vector3 right = new Vector3(currentDir.z, 0f, -currentDir.x);
        Vector3 back  = -currentDir;

        int r = UnityEngine.Random.Range(0, 3);
        Vector3 newDir = (r == 0) ? left : (r == 1) ? right : back;

        NewDirection.Value = newDir;

        // "이번 막힘 처리 1회 했다" 표시
        if (StuckCount != null)
            StuckCount.Value = 1;

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}
