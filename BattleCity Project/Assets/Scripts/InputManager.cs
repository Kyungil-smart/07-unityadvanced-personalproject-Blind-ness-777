using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction attackAction;

    private Vector2 moveInput;
    private bool fireRequested;

    public Vector2 MoveInput => moveInput;

    private void Awake()
    {
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();

        moveAction = InputSystem.actions["Move"];
        attackAction = InputSystem.actions["Attack"];
    }

    private void OnEnable()
    {
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
        attackAction.started += OnAttack;
    }

    private void OnDisable()
    {
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        attackAction.started -= OnAttack;
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        fireRequested = true;
    }

    // 발사 요청 소비. 한 번 읽으면 초기화. EnemyBrain.ConsumeFireRequested와 동일한 패턴
    public bool ConsumeFireRequested()
    {
        if (!fireRequested) return false;
        fireRequested = false;
        return true;
    }
}