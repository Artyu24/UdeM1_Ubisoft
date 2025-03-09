using System;
using System.Collections.Generic;
using NaughtyAttributes;
using NUnit.Framework;
using UnityEngine;

public class TeleportPlayers : MonoBehaviour
{
    [Required("Need a Teleport Point !"), SerializeField] private Transform _teleportationPoint;

    [SerializeField] private bool _needObject = false;
    [EnableIf("_needObject"), SerializeField] private ObjectBase _objectToGet;
    
    private List<PlayerInteraction> _playerInList = new List<PlayerInteraction>();
    private IGrabbable _objectGrabbed;

#if UNITY_EDITOR
    private void Awake()
    {
        DebugHelper.IsNull(_teleportationPoint, name, nameof(TeleportPlayers));
        if(_needObject)
            DebugHelper.IsNull(_objectToGet, name, nameof(TeleportPlayers));
    }
#endif
    
    private void LaunchTeleportation()
    {
        //Check TP Point not null & Player Number in the Collider
        if(_teleportationPoint == null || _playerInList.Count < 2)
            return;
        
        //If object needed, check if we have one
        if(_needObject && _objectGrabbed == null)
            return;
        
        //TP of Players
        for (int i = 0; i < _playerInList.Count; i++)
        {
            //If Player dont have the right object in head, we release it
            if(_playerInList[i].GrabbedObj != _objectGrabbed)
                _playerInList[i].GrabbedObj.OnRelease();
            
            _playerInList[i].OnPlayerInteractAction -= LaunchTeleportation;
            _playerInList[i].transform.position = _teleportationPoint.position + new Vector3(0.5f * i, 0, 0.5f * i);
        }

        //If Object not in Player Hand
        if (_objectGrabbed != null)
        {
            if (_objectGrabbed.GetObjectBase().transform.parent == null)
                _objectGrabbed.GetObjectBase().transform.position = _teleportationPoint.position;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Player Check
        PlayerInteraction playerInteract = other.GetComponent<PlayerInteraction>();
        if (playerInteract != null)
        {
            _playerInList.Add(playerInteract);
            playerInteract.OnPlayerInteractAction += LaunchTeleportation;
        }
        else //Object Check
        {
            IGrabbable objectFind = other.GetComponent<IGrabbable>();
            if (objectFind != null)
            {
                if (_objectToGet == (ObjectBase)objectFind)
                {
                    _objectGrabbed = objectFind;
                    
                }
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        //Player Check
        PlayerInteraction playerInteract = other.GetComponent<PlayerInteraction>();
        if (playerInteract != null)
        {
            if (_playerInList.Contains(playerInteract))
                _playerInList.Remove(playerInteract);

            playerInteract.OnPlayerInteractAction -= LaunchTeleportation;
        }
        else //Object Check
        {
            if (_objectGrabbed != null)
            {
                IGrabbable objectFind = other.GetComponent<IGrabbable>();
                if (objectFind != null)
                {
                    if(_objectToGet == (ObjectBase)objectFind)
                        _objectGrabbed = objectFind;
                }
            }
        }
    }
}
