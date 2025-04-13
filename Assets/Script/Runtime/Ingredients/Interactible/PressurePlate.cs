using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PressurePlate : MonoBehaviour
{
    private bool _isPushed;
    private int _playerNumberOnIt;

    [SerializeField] private UnityEvent _onPressurePlatePushed;
    [SerializeField] private UnityEvent _onPressurePlateRelease;
    public UnityEvent OnPressurePlatePushed => _onPressurePlatePushed;
    public UnityEvent OnPressurePlateRelease => _onPressurePlateRelease;

    private MeshRenderer _meshRenderer;
    
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.color =  Color.red;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerData>())
        {
            _playerNumberOnIt++;
            if (!_isPushed)
            {
                if(AudioManager.instance != null)
                    AudioManager.instance.PlayRandom(SoundState.SFX_PRESSURE_PLATE);
                
                _meshRenderer.material.color = Color.green;
                _isPushed = true;
                _onPressurePlatePushed.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerData>())
        {
            _playerNumberOnIt--;
            if (_playerNumberOnIt == 0)
            {
                _meshRenderer.material.color = Color.red;
                _isPushed = false;
                _onPressurePlateRelease.Invoke();
            }
        }
    }
}
