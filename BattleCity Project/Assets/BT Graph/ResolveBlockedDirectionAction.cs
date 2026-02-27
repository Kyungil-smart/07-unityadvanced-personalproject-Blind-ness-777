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

