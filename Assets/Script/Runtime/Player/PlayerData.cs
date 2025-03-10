using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Data")]
    private int _index;
    
    public bool SetupPlayerData(int index)
    {
        _index = index;

        name = "Player_" + (index + 1);
        
        return true;
    }
}
