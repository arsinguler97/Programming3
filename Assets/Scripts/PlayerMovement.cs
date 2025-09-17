using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    private Vector2 _move;
    private Vector3 _velocity;
    private float _speed = 5f;
    private float _rotationSpeed = 10f;
    private float _gravity = -9.81f;
    private float _jumpHeight = 2f;

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
            Debug.Log("Jump triggered");
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        _controller.Move(move * (_speed * Time.deltaTime));

        if (_controller.isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}