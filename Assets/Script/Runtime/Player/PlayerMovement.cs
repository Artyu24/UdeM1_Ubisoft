using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private PlayerData _data;
    [SerializeField] private Rigidbody _rb;

    [Header("Data")] 
    [SerializeField] private float _speed = 5;

    [Header("Maths")] 
    private Vector3 _movementInput;

#if UNITY_EDITOR
    private void Awake()
    {
        DebugHelper.IsNull(_data, name, nameof(PlayerMovement));
        DebugHelper.IsNull(_rb, name, nameof(PlayerMovement));
    }
#endif

    private void FixedUpdate()
    {
        if(_movementInput != Vector3.zero)
            _rb.MovePosition(_rb.position + _movementInput * Time.fixedDeltaTime * _speed);
    }
    
    public void OnPlayerMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _movementInput = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);
            transform.rotation = Quaternion.LookRotation(_movementInput, Vector3.up);
        }
        else
            _movementInput = Vector3.zero;            
    }
}
