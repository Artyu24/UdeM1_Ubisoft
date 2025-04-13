using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerData : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Transform _playerMesh;
    [SerializeField] private PlayerInteraction _playerInteraction;
    public PlayerInteraction Interaction => _playerInteraction;
    public Transform PlayerMesh => _playerMesh;
    [SerializeField] private GameObject _rocco;
    [SerializeField] private GameObject _munch;
    [SerializeField] private Animator _roccoAnim;
    [SerializeField] private Animator _munchAnim;
    public Animator AnimController
    {
        get
        {
            if(_index == 0)
                return _roccoAnim;
                
            return _munchAnim;
        }
    }

    [Header("Data")]
    private int _index;
    public int PlayerIndex => _index;
    private bool _isGrabByAI;
    public bool IsGrabByAI { get => _isGrabByAI; set => _isGrabByAI = value; }

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

        if (_index == 0)
            _rocco.SetActive(true);
        else if(_index == 1)
            _munch.SetActive(true);
        
        return true;
    }
}
