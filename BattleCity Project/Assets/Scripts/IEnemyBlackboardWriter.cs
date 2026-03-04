using UnityEngine;

public interface IEnemyBlackboardWriter
{
    void SetTarget(Transform target);
    void SetAttackRange(float attackRange);
    void SetIsForwardClear(bool isForwardClear);
    void SetBlockedTarget(Transform blockedTarget);
}