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

        // 센서가 BT 입력(블랙보드 대신 Brain)에 쓰도록 연결
        if (sensors != null && brain != null)
        {
            sensors.SetBlackboardWriter(brain);
        }
    }

    private void Update()
    {
        // 1) 센서: "가려는 방향" 기준으로 전방 체크 (transform.forward 쓰지 마)
        Vector3 forwardForSensor = brain != null ? brain.GetMoveDir() : transform.forward;
        sensors.Tick(forwardForSensor);

        // 2) 실행: 이동은 여기서만!
        if (movement != null && brain != null)
        {
            movement.Tick(brain.GetMoveDir());
        }

        // 3) 실행: 발사도 여기서만!
        if (brain != null)
        {
            bool fire = brain.ConsumeFireRequested();
            Debug.Log("[CTRL] ConsumeFireRequested=" + fire);

            if (fire)
            {
                Debug.Log("[CTRL] attack.Tick CALLED");
                if (attack != null) attack.Tick();
                else Debug.Log("[CTRL] attack is NULL");
            }
        }
    }
}