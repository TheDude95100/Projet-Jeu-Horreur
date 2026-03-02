using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystem : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerInputAction _action;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();        
        _action = new PlayerInputAction();
        _action.Player.Enable();
        _action.Player.Move.performed += Move_performed;
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("Je vois des trucs: " + obj);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_action.Player.Move.ReadValue<Vector2>().magnitude != 0)
        {
            Debug.Log("JE VOIS DES CHOSES");
        }
        Vector2 inputVector = _action.Player.Move.ReadValue<Vector2>();
        float speed = 5f;
        _rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }
}
