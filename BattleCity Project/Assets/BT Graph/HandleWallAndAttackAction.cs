using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "HandleWallAndAttack", story: "Handle wall for [ [Self] ] and attack based on [ [StageLevel] ]", category: "Action", id: "7a7ef598e7fe56988bbbf6e6834b546d")]
public partial class HandleWallAndAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<int> StageLevel;
    [SerializeReference] public BlackboardVariable<Vector3> DesiredDirection;
    [SerializeReference] public BlackboardVariable<int> WallStuckCounter;

    protected override Status OnStart()
    {
        WallStuckCounter.Value++;
        // 공격 확률: 기본 20% + 연속충돌 보너스 + 스테이지 보너스
        float prob = 0.2f + (WallStuckCounter.Value * 0.15f) + (StageLevel.Value * 0.05f);
        if (UnityEngine.Random.value < prob) { /* 사격 실행 */ Debug.Log("벽 사격!"); }

        Vector3 cur = DesiredDirection.Value;
        float r = UnityEngine.Random.value;
        if (r < 0.4f) DesiredDirection.Value = new Vector3(-cur.z, 0, cur.x); // 좌
        else if (r < 0.8f) DesiredDirection.Value = new Vector3(cur.z, 0, -cur.x); // 우
        else DesiredDirection.Value = -cur; // 뒤

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

