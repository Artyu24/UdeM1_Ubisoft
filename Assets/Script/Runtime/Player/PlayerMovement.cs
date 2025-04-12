using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] private float _iaPushCD = 2;

    [Header("Maths")] 
    private Vector3 _movementInput;

    [Header("Condition")] 
    private bool _isPushed = false;

    [Header("Grab related")]
    public UnityEvent OnGrab;
    public UnityEvent OnRelease;
    public bool _canMove = true;
    
#if UNITY_EDITOR
    private void Awake()
    {
        DebugHelper.IsNull(_data, name, nameof(PlayerMovement));
        DebugHelper.IsNull(_rb, name, nameof(PlayerMovement));
    }
#endif
    
    private void FixedUpdate()
    {
        if(_data.IsInTuto)
            return;
        
        if (_movementInput != Vector3.zero && !_isPushed && _canMove)
        {
            _data.AnimController.SetFloat("Speed", 1);

            if (_data.Timer.DoSound)
            {
                if(AudioManager.instance != null)
                    AudioManager.instance.PlayRandom(SoundState.SFX_RACOON_WALK);
                _data.Timer.DoSound = false;
            }
            
            Vector3 camFow = Camera.main.transform.forward;
            Vector3 camRig = Camera.main.transform.right;

            camFow.y = 0;
            camRig.y = 0;

            Vector3 fRel = _movementInput.z * camFow;
            Vector3 rRel = _movementInput.x * camRig;

            Vector3 moveDir = fRel + rRel;
            moveDir.Normalize();
            
            _rb.MovePosition(_rb.position + moveDir * Time.fixedDeltaTime * _moveSpeed);
            
            _data.PlayerMesh.rotation = Quaternion.LookRotation(moveDir, Vector3.up);
        }
        else
        {
            _data.AnimController.SetFloat("Speed", 0);
            
            if (_data.Timer.DoRandomSound)
            {
                if(AudioManager.instance != null)
                    AudioManager.instance.PlayRandom(SoundState.SFX_RACOON_IDLE);
                _data.Timer.DoRandomSound = false;
            }
        }
    }
    
    public void OnPlayerMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && ctx.ReadValue<Vector2>().sqrMagnitude > 0.1f)
        {
            _movementInput = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);
        }
        else
            _movementInput = Vector3.zero;            
    }

    public void OnIAPush(Vector3 iaPos)
    {
        _data.AnimController.SetTrigger("Pushed");
        
        Vector3 dir = transform.position - iaPos;
        _rb.AddForce(dir.normalized * _pushForce);
        StartCoroutine(PushedCoroutine());
    }

    private IEnumerator PushedCoroutine()
    {
        _isPushed = true;
        yield return new WaitForSeconds(_iaPushCD);
        _isPushed = false;
    }
    public void SetIsGrabed(bool isgrabed = true)
    {
        if (isgrabed)
        {
            _data.AnimController.SetBool("IAGrab", true);
            
            OnGrab.Invoke();
            _canMove = false;
            _rb.isKinematic = true;
        }
        else
        {
            _data.AnimController.SetBool("IAGrab", false);
            
            _canMove = true;
            _rb.isKinematic = false;
        }
    }
}
