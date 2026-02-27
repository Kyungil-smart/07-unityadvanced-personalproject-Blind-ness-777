using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "RollCombatFire", story: "Random roll against [CombatFireChance]", category: "Conditions", id: "fddc9ae4027db3f6ae001fd5fdb4a730")]
public partial class RollCombatFireCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> CombatFireChance;

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
