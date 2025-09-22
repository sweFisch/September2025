using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    float _speed = 5f;

    Vector2 inputMove;
    InputAction _inputActionMove;


    void Start()
    {
        _inputActionMove = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        inputMove = _inputActionMove.ReadValue<Vector2>();
        Vector3 moveVector = (Vector3)inputMove.normalized * _speed * Time.deltaTime;
        transform.position += moveVector;
    }
}
