using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyMovement movement;
    
    // 나중에 붙일 예정
    // [SerializeField] private EnemyBrain brain;

    private void Awake()
    {
        if (movement == null) movement = GetComponent<EnemyMovement>();
        // if (brain == null) brain = GetComponent<EnemyBrain>();
    }

    private void Update()
    {
        // 1) AI가 준 의도(임시로 하드코딩)
        // 나중에 : Vector3 moveDir = brain.GetMoveDir();
        Vector3 moveDir = Vector3.forward;  // Test용
        
        // 2) 실행
        movement.Tick(moveDir);
    }
}
