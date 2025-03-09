using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerInput> _playerList = new List<PlayerInput>();
    
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

        playerInput.gameObject.transform.position = transform.position;
        
        _playerList.Add(playerInput);
    }
}
