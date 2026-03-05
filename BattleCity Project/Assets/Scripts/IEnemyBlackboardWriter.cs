using UnityEngine;

// EnemySensors가 감지 결과를 Brain에 전달할 때 사용하는 인터페이스
public interface IEnemyBlackboardWriter
{
    void SetTarget(Transform target);
    void SetAttackRange(float attackRange);
    void SetIsForwardClear(bool isForwardClear);
    void SetBlockedTarget(Transform blockedTarget);
}