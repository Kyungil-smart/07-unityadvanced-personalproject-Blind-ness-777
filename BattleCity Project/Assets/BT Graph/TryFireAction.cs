using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(
    name: "TryFire",
    story: "Request fire sometimes",
    category: "Action",
    id: "TRY_FIRE_ACTION"
)]
public partial class TryFireAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    
    // 튜닝값(필요하면 Blackboard로 빼도 됨)
    [SerializeField] private float baseChance = 0.02f;      // 스테이지 1 기본 확률
    [SerializeField] private float perStageAdd = 0.02f;     // 스테이지당 증가
    [SerializeField] private float maxChance = 0.25f;       // 상한

    protected override Status OnStart()
    {
        // Debug.Log("[BT] TryFire ENTER");

        // Debug.Log($"[BT] TryFire Self is null? { (Self == null) }  Self.Value is null? { (Self != null && Self.Value == null) }");
        if (Self == null || Self.Value == null) return Status.Success;

        var brain = Self.Value.GetComponent<EnemyBrain>();
        // Debug.Log($"[BT] TryFire brain is null? { (brain == null) }");
        if (brain == null) return Status.Success;

        // Debug.Log($"[BT] TryFire tick canAttack={brain.GetCanAttack()} stage={brain.GetStageLevel()}");

        // 테스트: 무조건 발사
        brain.RequestFire();
        // Debug.Log("[BT] TryFire => FORCE FIRE");

        return Status.Success;
    }

    protected override Status OnUpdate() => Status.Success;

    protected override void OnEnd()
    {
    }
}

