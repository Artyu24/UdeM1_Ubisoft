using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportPlayers : MonoBehaviour
{
    [InfoBox("Scene must be in Build Settings"), SerializeField, Scene] private string _sceneName;

    [SerializeField] private bool _needObject = false;
    [EnableIf("_needObject"), SerializeField] private ObjectBase _objectToGet;
    
    private List<PlayerInteraction> _playerInList = new List<PlayerInteraction>();
    private IGrabbable _objectGrabbed;

    private void Awake()
    {
        if (!_needObject)
            _objectToGet = null;
#if UNITY_EDITOR
        else
            DebugHelper.IsNull(_objectToGet, name, nameof(TeleportPlayers));
#endif
    }
    
    private void LaunchTeleportation()
    {
        //Check TP Point not null & Player Number in the Collider
        if(_playerInList.Count < 2)
            return;
        
        //If object needed, check if we have one
        if(_needObject && _objectGrabbed == null)
            return;
        
        //Setup Players
        for (int i = 0; i < _playerInList.Count; i++)
        {
            //If Player dont have the right object in head, we release it
            if(_playerInList[i].GrabbedObj != _objectGrabbed)
                _playerInList[i].ReleaseObject();
            
            _playerInList[i].OnPlayerInteractAction -= LaunchTeleportation;
        }

        //If Object not in Player Hand
        if (_objectGrabbed != null)
        {
            if (_objectGrabbed.GetObjectBase().transform.parent == null)
                _playerInList[0].GrabObject(_objectGrabbed);
        }
        
        //Launch TP
        if(HUDManager.instance != null)
            HUDManager.instance.FadeInTransition(LoadScene);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(_sceneName);
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
