using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsForwardClear", story: "[ForwardClear] is true", category: "Conditions", id: "4ff2c2c48fcae013449f8c5d349a85e0")]
public partial class IsForwardClearCondition : Condition
{
    [SerializeReference] public BlackboardVariable<bool> ForwardClear;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
