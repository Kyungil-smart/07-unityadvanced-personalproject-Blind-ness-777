using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private EnemySensors sensors;
    [SerializeField] private EnemyBrain brain;
    [SerializeField] private EnemyAttack attack;
    [SerializeField] private float thinkInterval = 0.25f;
    private float nextThinkTime;

    private void Awake()
    {
        if (movement == null) movement = GetComponent<EnemyMovement>();
        if (sensors == null) sensors = GetComponent<EnemySensors>();
        if (brain == null) brain = GetComponent<EnemyBrain>();
        if (attack == null) attack = GetComponent<EnemyAttack>();

        if (sensors != null && brain != null)
            sensors.SetBlackboardWriter(brain);
    }

    // 감지 → 판단 → 이동 → 공격 순서로 매 프레임 실행
    private void Update()
    {
        if (sensors != null)
            sensors.Tick(brain.GetMoveDir());

        // thinkInterval마다 Brain 판단 실행
        if (Time.time >= nextThinkTime)
        {
            nextThinkTime = Time.time + thinkInterval;
            brain.Think();
        }

        if (movement != null)
            movement.Tick(brain.GetMoveDir());

        // Brain이 발사 요청했을 때만 Attack 실행
        if (brain.ConsumeFireRequested())
            attack?.Tick();
    }
}