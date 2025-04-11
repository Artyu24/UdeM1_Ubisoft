using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerData : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Animator _anim;
    public Animator AnimController => _anim;

    [Header("Data")]
    private int _index;

    [Header("Sound")] 
    [SerializeField] private MusicTimer _musicTimer;
    public MusicTimer Timer => _musicTimer;
    
    
    public bool SetupPlayerData(int index)
    {
        _index = index;

        name = "Player_" + (index + 1);
        
        return true;
    }
}
