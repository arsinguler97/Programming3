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
    
    //For AimCamera
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private float lookSensitivity = 2f;
    private float _pitch;
    private Vector2 _look;
    private bool _isAiming;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void SetMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }
    
    //For AimCamera
    public void SetLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }
    
    //For AimCamera
    public void SetAiming(bool aiming)
    {
        _isAiming = aiming;
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

        if (!_isAiming)
        {
            if (move.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        float currentSpeed = _isSprinting ? speed * sprintMultiplier : speed;
        _controller.Move(move * (currentSpeed * Time.deltaTime));

        if (_controller.isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
        
        //For AimCamera
        if (_isAiming)
        {
            transform.Rotate(Vector3.up * (_look.x * lookSensitivity));

            _pitch -= _look.y * lookSensitivity;
            _pitch = Mathf.Clamp(_pitch, -45f, 70f);

            cameraRoot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }
}
