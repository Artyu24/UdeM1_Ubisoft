using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerData _data;
    [SerializeField] private Transform _grabPos;
    
    [Header("Data Trigger Zone")] 
    [SerializeField, Min(0.1f)] private float _boxDist = 1;
    [SerializeField, Min(0.1f)] private float _boxWidth = 0.4f;
    [SerializeField, Min(0.1f)] private float _boxHeight = 1;

    [Header("Grab")] 
    [SerializeField] private float _grabCD = 0.4f;
    private float _grabTimer;
    private bool _canDoGrab;
    private IGrabbable _grabbedObj;
    public IGrabbable GrabbedObj { get => _grabbedObj; }

    [Header("Interact")] 
    public Action OnPlayerInteractAction;
    [SerializeField] private float _interactCD = 1f;
    private float _interactTimer;
    private bool _canInteract;
    //Check if in Interact CD / Object Grabbed / Action Possible
    public bool CanInteract => _canInteract && _grabbedObj == null && _canDoGrab;
    public bool DoPlayerInteractActionPossible => OnPlayerInteractAction != null;

    [Header("Search Object")] 
    [SerializeField] private FindTarget _findTargetPrefab;
    private bool _hasRightObjectInHand;
    [SerializeField] private float _findTargetCD = 1f;
    private float _findTargetTimer;
    private bool _canFindTarget;
    
    
#if UNITY_EDITOR
    private void Awake()
    {
        DebugHelper.IsNull(_grabPos, name, nameof(PlayerInteraction));
    }
#endif

    private void Update()
    {
        UpdateCD(ref _interactTimer, _interactCD, ref _canInteract);
        UpdateCD(ref _grabTimer, _grabCD, ref _canDoGrab);
        UpdateCD(ref _findTargetTimer, _findTargetCD, ref _canFindTarget);
    }

    private void UpdateCD(ref float timer, float maxCD, ref bool canDoAction)
    {
        if (timer > maxCD)
        {
            if (!canDoAction)
                canDoAction = true;
            return;
        }
        
        timer += Time.deltaTime;
    }

    public void OnPlayerGrab(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if(!_canDoGrab)
                return;
            
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
        if(!objectGrab.OnGrab(_grabPos))
            return;
        
        objectGrab.GetObjectBase().transform.DOLocalMove(Vector3.zero, 0.2f);
        objectGrab.GetObjectBase().transform.DOLocalRotate(Vector3.zero, 0.2f);
        
        _data.AnimController.SetBool("IsGrabbing", true);

        if(AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.SFX_GRAB);
        
        if (ReferenceEquals(PlayerManager.instance.TeleportPlayersObject.ObjectToGet, objectGrab))
        {
            PlayerManager.instance.IsObjectInHand = true;
            _hasRightObjectInHand = true;
        }
        
        _grabbedObj = objectGrab;
        
        _canDoGrab = false;
        _grabTimer = 0;
    }

    public void ReleaseObject()
    {
        if(_grabbedObj == null)
            return;
        
        _grabbedObj.OnRelease();
        
        _data.AnimController.SetBool("IsGrabbing", false);

        if (_hasRightObjectInHand)
        {
            PlayerManager.instance.IsObjectInHand = false;
            _hasRightObjectInHand = false;
        }
        
        _grabbedObj = null;
        
        _canDoGrab = false;
        _grabTimer = 0;
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
            
            //Cant Interact with an Item in Hand
            if(_grabbedObj != null || !_canInteract)
                return;
            
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
                        
                        _data.AnimController.SetTrigger("Interact");
                        
                        if(AudioManager.instance != null)
                            AudioManager.instance.PlayRandom(SoundState.SFX_RACOON_HIT);
                        
                        _interactTimer = 0;
                        _canInteract = false;
                        break;
                    }
                }
            }
        }
    }
    
    public void OnPlayerSearch(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if(PlayerManager.instance.TeleportPlayersObject == null || !_canFindTarget)
                return;

            FindTarget _findTarget = Instantiate(_findTargetPrefab, transform.position, Quaternion.identity);
            if(PlayerManager.instance.IsObjectInHand)
                _findTarget.Init(PlayerManager.instance.TeleportPlayersObject.transform.position);
            else
                _findTarget.Init(PlayerManager.instance.TeleportPlayersObject.ObjectToGet.transform.position);

            _findTargetTimer = 0;
            _canFindTarget = false;
        }
    }
}
