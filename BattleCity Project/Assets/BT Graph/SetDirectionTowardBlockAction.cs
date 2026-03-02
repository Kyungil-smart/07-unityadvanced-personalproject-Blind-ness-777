using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetDirectionTowardBlock", story: "[desiredDirection] = direction from [Self] to [BlockedTarget]", category: "Action", id: "6ea9b34c5a17f5bc8db9055fabbaa040")]
public partial class SetDirectionTowardBlockAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> DesiredDirection;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> BlockedTarget;

    protected override Status OnStart()
    {
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        if (DesiredDirection == null) return Status.Success;
        if (Self == null || Self.Value == null) return Status.Success;
        if (BlockedTarget == null || BlockedTarget.Value == null) return Status.Success;

        Vector3 from = Self.Value.transform.position;
        Vector3 to = BlockedTarget.Value.position;

        Vector3 dir = to - from;
        dir.y = 0f; // 탑뷰

        if (dir.sqrMagnitude <= 0.0001f) return Status.Success;

        dir.Normalize();

        // 4방향 스냅
        float absX = Mathf.Abs(dir.x);
        float absZ = Mathf.Abs(dir.z);

        if (absX >= absZ)
            DesiredDirection.Value = (dir.x >= 0f) ? Vector3.right : Vector3.left;
        else
            DesiredDirection.Value = (dir.z >= 0f) ? Vector3.forward : Vector3.back;

        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}