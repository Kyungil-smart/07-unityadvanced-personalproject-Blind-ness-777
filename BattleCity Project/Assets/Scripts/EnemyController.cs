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

    private void Update()
    {
        if (sensors != null) sensors.Tick(brain.GetMoveDir());

        if (Time.time >= nextThinkTime)
        {
            nextThinkTime = Time.time + thinkInterval;
            brain.Think();
        }

        if (movement != null)
            movement.Tick(brain.GetMoveDir());

        if (brain.ConsumeFireRequested())
            if (attack != null) attack.Tick();
    }
}