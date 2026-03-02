using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "KeepDirection", story: "Keep direction", category: "Action", id: "845b4c1169c382bd036e0c89e20e5e70")]
public partial class KeepDirectionAction : Action
{
    
    [SerializeReference] public BlackboardVariable<int> StuckCount;

    protected override Status OnStart()
    {
        return Status.Success;
    }

    protected override Status OnUpdate()
    {
        if (StuckCount != null)
        {
            StuckCount.Value = 0;
        }
        
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

