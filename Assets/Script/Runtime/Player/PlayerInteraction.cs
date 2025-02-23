using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _grabPos;

    [Header("Data")] 
    [SerializeField, Min(0.1f)] private float _dist = 1;
    [SerializeField, Min(0.1f)] private float _width = 0.4f;
    [SerializeField, Min(0.1f)] private float _height = 1;
    private IGrabbable _grabbedObj;
    
    public void OnPlayerGrab(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (_grabbedObj != null)
            {
                _grabbedObj.OnRelease();
                _grabbedObj = null;
                return;
            }
            
            RaycastHit[] hits = Physics.BoxCastAll(_grabPos.position, new Vector3(_width, _height, _width), _grabPos.forward, Quaternion.identity, _dist);
            if (hits.Length > 0)
            {
                foreach (var objectHit in hits)
                {
                    IGrabbable objectGrab = objectHit.transform.GetComponent<IGrabbable>();
                    if (objectGrab != null)
                    {
                        objectGrab.OnGrab(transform);
                        objectHit.transform.DOLocalMove(_grabPos.localPosition, 0.2f);

                        _grabbedObj = objectGrab;
                        break;
                    }
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_grabPos.position + _grabPos.forward, new Vector3(0.2f, 1f, 0.2f) * 2f);
    }
}
