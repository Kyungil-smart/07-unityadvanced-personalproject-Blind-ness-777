using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetDirectionTowardBase", story: "[desiredDirection] = direction from [Self] to [BaseTarget]", category: "Action", id: "PUT_UNIQUE_ID_HERE")]
public partial class SetDirectionTowardBaseAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> DesiredDirection;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Transform> BaseTarget;
    [SerializeReference] public BlackboardVariable<bool> IsForwardClear;

    protected override Status OnStart()
    {
        Debug.Log("SetDirectionTowardBase OnStart 호출됨");
        
        if (IsForwardClear != null && !IsForwardClear.Value)
            return Status.Success;
        
        if (DesiredDirection == null) return Status.Failure;
        if (Self == null || Self.Value == null) return Status.Failure;
        if (BaseTarget == null || BaseTarget.Value == null) return Status.Failure;

        Vector3 from = Self.Value.transform.position;
        Vector3 to = BaseTarget.Value.position;

        Vector3 dir = to - from;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f) return Status.Failure;

        dir.Normalize();

        // 4방 스냅
        float absX = Mathf.Abs(dir.x);
        float absZ = Mathf.Abs(dir.z);

        if (absX >= absZ)
            DesiredDirection.Value = (dir.x >= 0f) ? Vector3.right : Vector3.left;
        else
            DesiredDirection.Value = (dir.z >= 0f) ? Vector3.forward : Vector3.back;

        Debug.Log("desiredDirection 설정됨: " + DesiredDirection.Value);

        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}