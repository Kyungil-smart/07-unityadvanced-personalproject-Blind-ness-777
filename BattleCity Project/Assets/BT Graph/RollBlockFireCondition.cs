using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "RollBlockFire", story: "Roll with [BaseChance] + [StuckCount] * [PerStuck] . cap [MaxChance]", category: "Conditions", id: "e81370a065f961527803c54fdbe4afae")]
public partial class RollBlockFireCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> BaseChance;
    [SerializeReference] public BlackboardVariable<int> StuckCount;
    [SerializeReference] public BlackboardVariable<float> PerStuck;
    [SerializeReference] public BlackboardVariable<float> MaxChance;

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
