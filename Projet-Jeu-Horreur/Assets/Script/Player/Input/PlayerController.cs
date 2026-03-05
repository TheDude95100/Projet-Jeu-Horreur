using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _maxSpeed = 15f;
    [SerializeField] private float _acceleration = 8f;
    [SerializeField] private float _deceleration = 8f;

    private Rigidbody _rb;
    private PlayerInputAction _action;
    private Vector2 _inputVector;
    private Vector3 _currentForce;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _action = new PlayerInputAction();
        _action.Player.Enable();
        _action.Player.Move.performed += ctx => _inputVector = ctx.ReadValue<Vector2>();
        _action.Player.Move.canceled += ctx => _inputVector = Vector2.zero;
    }

    private void OnDestroy()
    {
        _action.Player.Move.performed -= ctx => _inputVector = ctx.ReadValue<Vector2>();
        _action.Player.Move.canceled -= ctx => _inputVector = Vector2.zero;
        _action.Dispose();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ClampVelocity();
    }

    private void ApplyMovement()
    {
        if (_inputVector != Vector2.zero)
        {
            Vector3 targetForce = new Vector3(_inputVector.x, 0f, _inputVector.y) * _moveSpeed;
            _currentForce = Vector3.Lerp(_currentForce, targetForce, _acceleration * Time.fixedDeltaTime);
            _rb.AddForce(_currentForce, ForceMode.VelocityChange);
        }
        else
        {
            Vector3 flatVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            _currentForce = Vector3.Lerp(_currentForce, Vector3.zero, _deceleration * Time.fixedDeltaTime);
            _rb.AddForce(-flatVelocity * _deceleration, ForceMode.Force);
        }
    }

    private void ClampVelocity()
    {
        Vector3 flatVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);

        if (flatVelocity.magnitude > _maxSpeed)
        {
            Vector3 clamped = flatVelocity.normalized * _maxSpeed;
            _rb.linearVelocity = new Vector3(clamped.x, _rb.linearVelocity.y, clamped.z);
        }
    }

    private void OnEnable() => _action.Player.Enable();
    private void OnDisable() => _action.Player.Disable();
}

