using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetTargetDirection", story: "Update [ [DesiredDirection] ] toward [ [BaseTarget] ]", category: "Action", id: "f4015affbdd834b8028e7667e3313963")]
public partial class SetTargetDirectionAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> DesiredDirection;
    [SerializeReference] public BlackboardVariable<Transform> BaseTarget;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

