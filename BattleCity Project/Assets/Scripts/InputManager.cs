using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    
    private InputAction moveAction;
    private InputAction attackAction;
    
    private Vector2 moveInput;
    private bool fireRequested;
    
    public Vector2 MoveInput { get => moveInput; }

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
        Debug.Log($"{moveInput}");
    }
    
    private void OnAttack(InputAction.CallbackContext ctx)
    {
        fireRequested = true;
    }
    
    // 요청 "꺼내기(소모)"
    public bool ConsumeFireRequested()
    {
        if (!fireRequested) return false;
        
        fireRequested = false;
        return true;
    }
}