using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private enum Axis { None, X, Y }

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private LayerMask movementBlockMask;

    private Vector2 input;
    private Vector2 prevInput;
    private Axis lastAxis = Axis.None;
    private Vector3 direction;
    private Vector3 lookDir;

    private const float DeadZoneSqr = 0.01f;
    private const float AxisPressThreshold = 0.1f;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.isKinematic = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // 탑뷰 환경에서 Y축 이동 및 회전 고정
        rb.constraints = RigidbodyConstraints.FreezePositionY
                         | RigidbodyConstraints.FreezeRotationX
                         | RigidbodyConstraints.FreezeRotationZ;
    }

    // 입력 수신 후 축 고정, 회전, 이동 방향 계산. 물리 적용은 FixedUpdate에서
    public void Tick(Vector2 moveInput)
    {
        input = moveInput;
        if (input.sqrMagnitude < DeadZoneSqr)
        {
            lastAxis = Axis.None;
            prevInput = moveInput;
            direction = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            return;
        }

        ApplyAxisLock();
        RotationLogic();

        direction = Vector3.forward * input.y + Vector3.right * input.x;

        if (lookDir != Vector3.zero)
            rb.MoveRotation(Quaternion.LookRotation(lookDir));

        prevInput = moveInput;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = Vector3.zero;
        if (direction == Vector3.zero) return;

        // Trigger 콜라이더 무시하고 벽 감지. 충돌 예상 시 이동 중단
        if (rb.SweepTest(direction, out _, speed * Time.fixedDeltaTime + 0.2f, QueryTriggerInteraction.Ignore))
            return;

        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    // 대각선 입력 방지. 새로 눌린 축 우선, 느려짐 방지를 위해 입력값 정규화
    private void ApplyAxisLock()
    {
        bool isXPressed = Mathf.Abs(input.x) > AxisPressThreshold;
        bool isYPressed = Mathf.Abs(input.y) > AxisPressThreshold;
        bool wasXPressed = Mathf.Abs(prevInput.x) > AxisPressThreshold;
        bool wasYPressed = Mathf.Abs(prevInput.y) > AxisPressThreshold;

        bool xJustPressed = isXPressed && !wasXPressed;
        bool yJustPressed = isYPressed && !wasYPressed;

        if (xJustPressed && !yJustPressed) lastAxis = Axis.X;
        else if (yJustPressed && !xJustPressed) lastAxis = Axis.Y;
        else if (xJustPressed && yJustPressed)
            lastAxis = (Mathf.Abs(input.x) >= Mathf.Abs(input.y)) ? Axis.X : Axis.Y;
        else if (lastAxis == Axis.None)
            lastAxis = (Mathf.Abs(input.x) >= Mathf.Abs(input.y)) ? Axis.X : Axis.Y;

        if (lastAxis == Axis.X && !isXPressed && isYPressed) lastAxis = Axis.Y;
        else if (lastAxis == Axis.Y && !isYPressed && isXPressed) lastAxis = Axis.X;

        if (lastAxis == Axis.X) { input.y = 0f; input.x = Mathf.Sign(input.x); }
        else if (lastAxis == Axis.Y) { input.x = 0f; input.y = Mathf.Sign(input.y); }
    }

    private void RotationLogic()
    {
        if (lastAxis == Axis.X)
            lookDir = input.x < 0f ? Vector3.left : Vector3.right;

        if (lastAxis == Axis.Y)
            lookDir = input.y < 0f ? Vector3.back : Vector3.forward;
    }
}