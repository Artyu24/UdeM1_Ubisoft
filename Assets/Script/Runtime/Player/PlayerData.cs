using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerData : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Transform _playerMesh;
    public Transform PlayerMesh => _playerMesh;
    [SerializeField] private Animator _anim;
    public Animator AnimController => _anim;

    [Header("Data")]
    private int _index;
    public int PlayerIndex => _index;

    [Header("Tutorial")] 
    [SerializeField] private PlayerTutorial _pTutorial;
    public PlayerTutorial PTutorial => _pTutorial;
    private bool _isInTuto;
    public bool IsInTuto { get => _isInTuto; set => _isInTuto = value; }
    private bool _isReadyToPlay;
    public bool IsReadyToPlay { get => _isReadyToPlay; set => _isReadyToPlay = value; }
    
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
