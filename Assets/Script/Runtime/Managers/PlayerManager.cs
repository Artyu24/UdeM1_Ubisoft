using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public List<PlayerInput> _playerList {  get; private set; }

    //Object Search
    private bool _isObjectInHand = false;
    public bool IsObjectInHand { get => _isObjectInHand; set => _isObjectInHand = value; }
    private TeleportPlayers _teleportPlayersObject;
    public TeleportPlayers TeleportPlayersObject => _teleportPlayersObject;

    public bool isRestoLevel = false;

    private void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
        {
            instance.TeleportPlayer(transform.position);
            Destroy(gameObject);
            return;
        }

        _playerList = new List<PlayerInput>();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += (arg0, mode) => SceneLoadedInit();
        PlayerPrefs.SetString("lastScene", "");
        SceneLoadedInit();
    }

    public void OnPlayerJoin(PlayerInput playerInput)
    {
        if (_playerList.Count >= 2)
        {
            Destroy(playerInput.gameObject);
            return;
        }
        
        PlayerData pData = playerInput.gameObject.GetComponent<PlayerData>();
        if(DebugHelper.IsNull(pData, name, nameof(PlayerManager)))
           return;
        
        pData.SetupPlayerData(playerInput.playerIndex);

        playerInput.transform.position = transform.position;
        
        _playerList.Add(playerInput);



        // for resto level

        if (isRestoLevel)
        {
            if(_playerList.Count == 2)
            {
               
            }
        }
    }

    private void SceneLoadedInit()
    {
        _isObjectInHand = false;
        
        //Check if we are in the Hub Scene
        HubManager hub = GameObject.FindFirstObjectByType(typeof(HubManager)) as HubManager;
        if (hub != null)
        {
            //If its hub scene, teleport player to the right spawn depending on last scene
            string oldSceneName = PlayerPrefs.GetString("lastScene");
            Transform tpPoint = hub.TeleportPlayer(oldSceneName);

            if (tpPoint == null)
                tpPoint = transform;
            
            for (int i = 0; i < _playerList.Count; i++)
            {
                _playerList[i].transform.position = tpPoint.position + new Vector3(1.5f * i, 0, 1.5f * i);
            }
        }
        else
        {
            //Else, we keep the scene name
            PlayerPrefs.SetString("lastScene", SceneManager.GetActiveScene().name);
         
            //Check Object Find
            _teleportPlayersObject = GameObject.FindFirstObjectByType(typeof(TeleportPlayers)) as TeleportPlayers;
        }
    }

    private void TeleportPlayer(Vector3 position)
    {
        if(GameObject.FindFirstObjectByType(typeof(HubManager)))
            return;
        
        for (int i = 0; i < _playerList.Count; i++)
        {
            _playerList[i].transform.position = position + new Vector3(0.75f * i, 0, 0.75f * i);
        }
    }
}
