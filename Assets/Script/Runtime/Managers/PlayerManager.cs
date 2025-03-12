using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public List<PlayerInput> _playerList {  get; private set; }

    private void Awake()
    {
        
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        _playerList = new List<PlayerInput>();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += (arg0, mode) => SceneLoadedInit();
        PlayerPrefs.SetString("lastScene", "");
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
    }

    private void SceneLoadedInit()
    {
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

            foreach (PlayerInput player in _playerList)
            {
                player.transform.position = transform.position;
            }
        }
    }
}
