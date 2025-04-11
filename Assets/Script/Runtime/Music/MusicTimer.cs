using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MusicTimer : MonoBehaviour
{
    [Header("Sound")]
    private float _timerSound;
    [SerializeField] private float _timerSoundCd = 0.5f;
    private bool _doSound;
    public bool DoSound { get => _doSound; set => _doSound = value; }

    [Header("Random")]
    private float _timerRandomSound;
    [SerializeField] private float _timerRandomSoundMinCd = 2f;
    [SerializeField] private float _timerRandomSoundMaxCd = 5f;
    private float _timerRandomSoundCd = 2f;
    private bool _doRandomSound;
    public bool DoRandomSound { get => _doRandomSound; set => _doRandomSound = value; }

    private void Awake()
    {
        _timerRandomSoundCd = Random.Range(_timerRandomSoundMinCd, _timerRandomSoundMaxCd);
    }

    private void Update()
    {
        //Classic
        _timerSound += Time.deltaTime;

        if (_timerSound > _timerSoundCd)
        {
            _timerSound = 0;
            _doSound = true;
        }
        
        //Random
        _timerRandomSound += Time.deltaTime;

        if (_timerRandomSound > _timerRandomSoundCd)
        {
            _timerRandomSound = 0;
            _doRandomSound = true;
            _timerRandomSoundCd = Random.Range(2f, 5f);
        }
    }
}
