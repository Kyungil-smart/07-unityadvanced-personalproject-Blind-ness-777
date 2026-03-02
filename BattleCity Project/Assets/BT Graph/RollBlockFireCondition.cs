using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(
    name: "RollBlockFire",
    story: "Roll with [BaseChance] + [StuckCount] * [PerStuck] . cap [MaxChance]",
    category: "Conditions",
    id: "e81370a065f961527803c54fdbe4afae"
)]
public partial class RollBlockFireCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> BaseChance;
    [SerializeReference] public BlackboardVariable<int> StuckCount;
    [SerializeReference] public BlackboardVariable<float> PerStuck;
    [SerializeReference] public BlackboardVariable<float> MaxChance;

    // ★ 추가: 막고 있는 대상(센서가 써주는 값)
    [SerializeReference] public BlackboardVariable<Transform> BlockedTarget;

    public override bool IsTrue()
    {
        // 0) blockedTarget 없으면 발사 판단 자체를 안 함
        if (BlockedTarget == null || BlockedTarget.Value == null) return false;

        Transform t = BlockedTarget.Value;

        // 1) Water는 "이동만 막고 총알 통과" 규칙이므로 절대 발사하지 않음
        int waterLayer = LayerMask.NameToLayer("Water");
        if (waterLayer != -1 && t.gameObject.layer == waterLayer) return false;

        // 2) Breakable 타일만 발사 허용
        Tiles tiles = t.GetComponent<Tiles>();
        if (tiles == null) return false;

        if (!tiles.IsBreakable()) return false;

        // 3) 확률 계산
        float baseChance = (BaseChance != null) ? BaseChance.Value : 0f;
        int stuckCount = (StuckCount != null) ? StuckCount.Value : 0;
        float perStuck = (PerStuck != null) ? PerStuck.Value : 0f;
        float maxChance = (MaxChance != null) ? MaxChance.Value : 1f;

        float chance = baseChance + stuckCount * perStuck;
        chance = Mathf.Clamp(chance, 0f, maxChance);

        return UnityEngine.Random.value < chance;
    }

    public override void OnStart() { }
    public override void OnEnd() { }
}