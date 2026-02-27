using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RequestFire", story: "Sets [fireRequested] = true", category: "Action", id: "161e6b6a4c3b3ae226f0fb6be6be36b1")]
public partial class RequestFireAction : Action
{
    [SerializeReference] public BlackboardVariable<bool> FireRequested;

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

