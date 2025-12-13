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

    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float lookSensitivity = 2f;

    private float _pitch;
    private Vector2 _look;
    private bool _isAiming;

    private DeployManager _deployManager;
    private PlayerStamina _stamina;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _deployManager = FindFirstObjectByType<DeployManager>();
        _stamina = GetComponent<PlayerStamina>();
    }

    public void SetMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    public void SetLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }

    public void SetAiming(bool aiming)
    {
        _isAiming = aiming;
    }

    public void SetJump(InputAction.CallbackContext context)
    {
        if (context.performed && _controller.isGrounded)
        {
            bool canJump = _stamina == null || _stamina.TryConsumeJump();
            if (canJump)
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void SetSprint(InputAction.CallbackContext context)
    {
        if (context.started)
            _isSprinting = _stamina == null || _stamina.CanStartSprint();
        if (context.canceled)
            _isSprinting = false;
    }

    public void DeployFireCannon(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) _deployManager?.DeployFireCannon();
    }

    public void DeployIceCannon(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) _deployManager?.DeployIceCannon();
    }

    public void DeploySpikeTrap(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) _deployManager?.DeploySpikeTrap();
    }

    public void DeployBarrier(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) _deployManager?.DeployBarrier();
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
        if (_stamina != null)
        {
            if (_isSprinting && !_stamina.HasStamina)
                _isSprinting = false;

            _stamina.SetSprinting(_isSprinting);
            _stamina.SetAiming(_isAiming);
        }

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 move = camForward * _move.y + camRight * _move.x;

        if (!_isAiming && move.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);
        }

        float currentSpeed = _isSprinting ? speed * sprintMultiplier : speed;
        _controller.Move(move * (currentSpeed * Time.deltaTime));

        if (_controller.isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);

        if (_isAiming)
        {
            transform.Rotate(Vector3.up * (_look.x * lookSensitivity));

            _pitch -= _look.y * lookSensitivity;
            _pitch = Mathf.Clamp(_pitch, -45f, 70f);

            cameraRoot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }
}
