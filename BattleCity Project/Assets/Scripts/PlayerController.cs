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

    private void Update()
    {
        playerMovement.Tick(inputManager.MoveInput);
        playerAttack.Tick();
        Debug.Log($"{inputManager.MoveInput}");
    }

    private void CameraChange()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            // TODO: 카메라 전환
        }
    }
}