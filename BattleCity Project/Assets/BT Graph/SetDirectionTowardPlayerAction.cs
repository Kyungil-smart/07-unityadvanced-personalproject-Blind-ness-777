using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetDirectionTowardPlayer", story: "[desiredDirection] = direction from [Self] to [Target]", category: "Action", id: "0174340141690dc42254abdc8ef35bb8")]
public partial class SetDirectionTowardPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> DesiredDirection;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    protected override Status OnStart()
    {
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        
        if (DesiredDirection == null) return Status.Success;
        if (Self == null || Self.Value == null) return Status.Success;
        if (Target == null || Target.Value == null) return Status.Success;

        Vector3 from = Self.Value.transform.position;
        Vector3 to = Target.Value.position;

        Vector3 dir = to - from;
        dir.y = 0f; // 탑뷰: Y 무시

        if (dir.sqrMagnitude <= 0.0001f) return Status.Success;

        dir.Normalize();

        // 4방향 스냅 (배틀시티 규칙)
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