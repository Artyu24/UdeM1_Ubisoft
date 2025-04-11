using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private Animator _anim;
    public Animator AnimController => _anim;

    [Header("Data")]
    private int _index;

    private float _timerSound;
    private float _timerSoundCD = 0.5f;
    private bool _doSound;
    public bool DoSound { get => _doSound; set => _doSound = value; }

    public bool SetupPlayerData(int index)
    {
        _index = index;

        name = "Player_" + (index + 1);
        
        return true;
    }

    private void Update()
    {
        _timerSound += Time.deltaTime;

        if (_timerSound > _timerSoundCD)
        {
            _timerSound = 0;
        }
    }
}
