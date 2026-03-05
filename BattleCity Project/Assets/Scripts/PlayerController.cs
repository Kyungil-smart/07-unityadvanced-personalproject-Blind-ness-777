using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerAttack playerAttack;

    private void Awake()
    {
        if (inputManager == null) inputManager = GetComponent<InputManager>();
        if (playerMovement == null) playerMovement = GetComponent<PlayerMovement>();
        if (playerAttack == null) playerAttack = GetComponent<PlayerAttack>();
    }

    // 입력 → 이동 → 공격 순서로 매 프레임 실행
    private void Update()
    {
        playerMovement.Tick(inputManager.MoveInput);
        playerAttack.Tick();
    }
}