using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum Axis { None, X, Y }
    
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float speed = 2.0f;
    
    private Vector2 input;
    private Vector2 prevInput;
    private Axis lastAxis = Axis.None;
    private Vector3 direction;
    private Vector3 lookDir;

    private const float DeadZoneSqr = 0.01f;
    private const float AxisPressThreshold = 0.1f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    
    public void Tick(Vector2 moveInput)
    {
        input = moveInput;
        if (input.sqrMagnitude < DeadZoneSqr)
        {
            lastAxis = Axis.None;
            prevInput = moveInput;
            return;
        }
        
        ApplyAxisLock();
        RotationLogic();
        
        direction = Vector3.forward * input.y + Vector3.right * input.x;
        characterController.Move(direction * speed * Time.deltaTime);

        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }
        
        prevInput = moveInput;
    }

    private void ApplyAxisLock()
    {
        bool isXPressed = Mathf.Abs(input.x) > AxisPressThreshold;
        bool isYPressed = Mathf.Abs(input.y) > AxisPressThreshold;
        bool wasXPressed = Mathf.Abs(prevInput.x) > AxisPressThreshold;
        bool wasYPressed = Mathf.Abs(prevInput.y) > AxisPressThreshold;
        
        bool xJustPressed = isXPressed && !wasXPressed;
        bool yJustPressed = isYPressed && !wasYPressed;
        
        // 새로 눌린 축 우선
        if (xJustPressed && !yJustPressed) lastAxis = Axis.X;
        else if (yJustPressed && !xJustPressed) lastAxis = Axis.Y;
        else if (xJustPressed && yJustPressed)
        {
            lastAxis = (Mathf.Abs(input.x) >= Mathf.Abs(input.y)) ? Axis.X : Axis.Y;
        }
        else if (lastAxis == Axis.None)
        {
            lastAxis = (Mathf.Abs(input.x) >= Mathf.Abs(input.y)) ? Axis.X : Axis.Y;
        }
        
        // 멈춤 방지
        if (lastAxis == Axis.X && !isXPressed && isYPressed) lastAxis = Axis.Y;
        else if (lastAxis == Axis.Y && !isYPressed && isXPressed) lastAxis = Axis.X;
        
        // 대각선 제거
        if (lastAxis == Axis.X) input.y = 0f;
        else if (lastAxis == Axis.Y) input.x = 0f;
        
        // 대각 입력 시 느려짐 방지
        if (lastAxis == Axis.X) input.x = Mathf.Sign(input.x);
        else if (lastAxis == Axis.Y) input.y = Mathf.Sign(input.y);
    }
    
    // 캐릭터 회전 로직
    private void RotationLogic()
    {
        if (lastAxis == Axis.X)
        {
            if (input.x < 0f)
            {
                lookDir = Vector3.left;
            }
            else lookDir = Vector3.right;
        }

        if (lastAxis == Axis.Y)
        {
            if (input.y < 0f)
            {
                lookDir = Vector3.back;
            }
            else lookDir = Vector3.forward;
        }
    }
}