using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyMovement movement;
    [SerializeField] private EnemySensors sensors;
    [SerializeField] private EnemyBrain brain;
    [SerializeField] private EnemyAttack attack;

    private void Awake()
    {
        if (movement == null) movement = GetComponent<EnemyMovement>();
        if (sensors == null) sensors = GetComponent<EnemySensors>();
        if (brain == null) brain = GetComponent<EnemyBrain>();
        if (attack == null) attack = GetComponent<EnemyAttack>();
    }

    private void Update()
    {
        // 센서 전방 체크는 보통 "현재 바라보는 방향"
        sensors.Tick(transform.forward);

        // 두뇌(의사결정)
        brain.Tick();

        // 실행
        movement.Tick(brain.GetMoveDir());
        
        // 발사
        if (brain.ConsumeFireRequested())
        {
            if (attack != null) attack.Tick();
        }
    }
}
