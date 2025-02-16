using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private List<PlayerInput> _playerList = new List<PlayerInput>();
    
    public void OnPlayerJoin(PlayerInput playerInput)
    {
        PlayerData pData = playerInput.gameObject.GetComponent<PlayerData>();
        if(DebugHelper.IsNull(pData, name, nameof(PlayerManager)))
           return;
        
        pData.SetupPlayerData(playerInput.playerIndex);
        
        _playerList.Add(playerInput);
    }
}
