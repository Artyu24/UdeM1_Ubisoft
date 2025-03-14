using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _grabPos;
    
    [Header("Data Trigger Zone")] 
    [SerializeField, Min(0.1f)] private float _boxDist = 1;
    [SerializeField, Min(0.1f)] private float _boxWidth = 0.4f;
    [SerializeField, Min(0.1f)] private float _boxHeight = 1;

    [Header("Grab")] 
    private IGrabbable _grabbedObj;
    public IGrabbable GrabbedObj { get => _grabbedObj; }

    [Header("Interact")] 
    public Action OnPlayerInteractAction; 

#if UNITY_EDITOR
    private void Awake()
    {
        DebugHelper.IsNull(_grabPos, name, nameof(PlayerInteraction));
    }
#endif

    public void OnPlayerGrab(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (_grabbedObj != null)
            {
                ReleaseObject();
                return;
            }
            
            RaycastHit[] hits = Physics.BoxCastAll(_grabPos.position, new Vector3(_boxWidth, _boxHeight, _boxWidth), _grabPos.forward, Quaternion.identity, _boxDist);
            if (hits.Length > 0)
            {
                foreach (var objectHit in hits)
                {
                    IGrabbable objectGrab = objectHit.transform.GetComponent<IGrabbable>();
                    if (objectGrab != null)
                    {
                        GrabObject(objectGrab);
                        break;
                    }
                }
            }
        }
    }

    public void GrabObject(IGrabbable objectGrab)
    {
        objectGrab.OnGrab(transform);
        objectGrab.GetObjectBase().transform.DOLocalMove(_grabPos.localPosition, 0.2f);

        _grabbedObj = objectGrab;
    }

    public void ReleaseObject()
    {
        _grabbedObj.OnRelease();
        _grabbedObj = null;
    }

    public void OnPlayerInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            //If an Action is possible
            if (OnPlayerInteractAction != null)
            {
                OnPlayerInteractAction.Invoke();
                return;
            }
            
            //Else, find some object to interact with
            RaycastHit[] hits = Physics.BoxCastAll(_grabPos.position, new Vector3(_boxWidth, _boxHeight, _boxWidth), _grabPos.forward, Quaternion.identity, _boxDist);
            if (hits.Length > 0)
            {
                foreach (var objectHit in hits)
                {
                    IInteractable objectInteract = objectHit.transform.GetComponent<IInteractable>();
                    if (objectInteract != null)
                    {
                        objectInteract.Interact();
                        break;
                    }
                }
            }
        }
    }
    
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireCube(_grabPos.position + _grabPos.forward, new Vector3(0.2f, 1f, 0.2f) * 2f);
    //}
}
