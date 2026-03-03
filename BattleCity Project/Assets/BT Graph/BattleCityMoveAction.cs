using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BattleCityMove", story: "Move [ [Self] ] toward [ [DesiredDirection] ]", category: "Action", id: "56cb632eb50cc5f42ebcba2ce220354b")]
public partial class BattleCityMoveAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Vector3> DesiredDirection;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Transform t = Self.Value.transform;
        t.position += DesiredDirection.Value * 3f * Time.deltaTime;
        if (DesiredDirection.Value != Vector3.zero) t.forward = DesiredDirection.Value;
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

