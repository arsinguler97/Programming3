using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    private Vector2 _move;
    private Vector3 _velocity;
    private bool _isSprinting;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private Transform cameraTransform;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void SetMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }
    

    public void SetJump(InputAction.CallbackContext context)
    {
        if (context.performed && _controller.isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void SetSprint(InputAction.CallbackContext context)
    {
        if (context.started) _isSprinting = true;
        if (context.canceled) _isSprinting = false;
    }
    
    public void SetAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GetComponentInChildren<PlayerAttack>()?.TryAttack();
        }
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * _move.y + camRight * _move.x;

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        float currentSpeed = _isSprinting ? speed * sprintMultiplier : speed;
        _controller.Move(move * (currentSpeed * Time.deltaTime));

        if (_controller.isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
