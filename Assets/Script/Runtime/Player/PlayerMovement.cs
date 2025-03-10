using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private PlayerData _data;
    [SerializeField] private Rigidbody _rb;
    
    [Header("Data")] 
    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private float _pushForce = 300;

    [Header("Maths")] 
    private Vector3 _movementInput;

    [Header("Condition")] 
    private bool _isPushed = false;

#if UNITY_EDITOR
    private void Awake()
    {
        DebugHelper.IsNull(_data, name, nameof(PlayerMovement));
        DebugHelper.IsNull(_rb, name, nameof(PlayerMovement));
    }
#endif

    private void FixedUpdate()
    {
        if(_movementInput != Vector3.zero && !_isPushed)
            _rb.MovePosition(_rb.position + _movementInput * Time.fixedDeltaTime * _moveSpeed);
    }
    
    public void OnPlayerMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && ctx.ReadValue<Vector2>().sqrMagnitude > 0.1f)
        {
            _movementInput = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);
            transform.rotation = Quaternion.LookRotation(_movementInput, Vector3.up);
        }
        else
            _movementInput = Vector3.zero;            
    }

    public void OnIAPush(Vector3 iaPos)
    {
        Vector3 dir = transform.position - iaPos;
        _rb.AddForce(dir.normalized * _pushForce);
        StartCoroutine(PushedCoroutine());
    }

    private IEnumerator PushedCoroutine()
    {
        _isPushed = true;
        yield return new WaitForSeconds(0.5f);
        _isPushed = false;
    }
}
