using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Throne : MonoBehaviour
{
    public static Throne instance;
    
    public static event Action<int> OnThroneUpdated;
    [SerializeField] private int ObjectCounter;

    [SerializeField] private SerializedDictionary<ObjectTypeEnum, ThroneObjectData> _throneObjectDataDict = new SerializedDictionary<ObjectTypeEnum, ThroneObjectData>();
    
    private List<PlayerInteraction> _playerInList = new List<PlayerInteraction>();
    private IGrabbable _objectGrabbed;
    
    private void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += (arg0, mode) => SceneLoadedInit();
    }
    
    private void AddObject()
    {
        if(_objectGrabbed == null)
            return;
        
        // new object
        ObjectCounter++;
        Debug.Log("Throne Count : " + ObjectCounter);
        OnThroneUpdated?.Invoke(ObjectCounter); // broadcast event
        
        if (_throneObjectDataDict.ContainsKey(_objectGrabbed.GetObjectBase().ObjectType))
        {
            for (int i = 0; i < _playerInList.Count; i++)
            {
                _playerInList[i].ReleaseObject();
            }
            
            _objectGrabbed.OnGrab(transform);

            ObjectTypeEnum objType = _objectGrabbed.GetObjectBase().ObjectType;
            _objectGrabbed.GetObjectBase().transform.DOLocalMove(_throneObjectDataDict[objType].Position, 0.2f);
            _objectGrabbed.GetObjectBase().transform.DOLocalRotate(_throneObjectDataDict[objType].Rotation, 0.2f);

            _objectGrabbed = null;
        }
    }
    
    private void SceneLoadedInit()
    {
        //Check if we are in the Hub Scene
        HubManager hub = GameObject.FindFirstObjectByType(typeof(HubManager)) as HubManager;
        if (hub != null)
        {
            gameObject.SetActive(true);
            OnThroneUpdated?.Invoke(ObjectCounter);
        }
        else
            gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //Player Check
        PlayerInteraction playerInteract = other.GetComponent<PlayerInteraction>();
        if (playerInteract != null)
        {
            _playerInList.Add(playerInteract);
            playerInteract.OnPlayerInteractAction += AddObject;
        }
        else //Object Check
        {
            IGrabbable objectFind = other.GetComponent<IGrabbable>();
            if (objectFind != null)
            {
                _objectGrabbed = objectFind;
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

            playerInteract.OnPlayerInteractAction -= AddObject;
        }
        else //Object Check
        {
            if (_objectGrabbed != null)
            {
                IGrabbable objectFind = other.GetComponent<IGrabbable>();
                if (objectFind != null)
                {
                    _objectGrabbed = objectFind;
                }
            }
        }
    }
    
    [Serializable]
    private struct ThroneObjectData
    {
        [SerializeField] private Vector3 _position;
        [SerializeField] private Vector3 _rotation;
        public Vector3 Position => _position;
        public Vector3 Rotation => _rotation;
    }

    [Button]
    public void EditorTest()
    {
        ObjectCounter++;
        Debug.Log("Throne Count : " + ObjectCounter);
        OnThroneUpdated?.Invoke(ObjectCounter); // broadcast event
    }
}
