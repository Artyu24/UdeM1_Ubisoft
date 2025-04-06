using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoublePressurePlate : MonoBehaviour
{
    [SerializeField] private List<PressurePlate> _pressurePlatesList = new List<PressurePlate>();

    private int _playerCount;
    
    [SerializeField] private UnityEvent _onDoublePressurePlatePushed;
    
    private void Awake()
    {
        foreach (PressurePlate pressurePlate in _pressurePlatesList)
        {
            pressurePlate.OnPressurePlatePushed.AddListener(PressurePlatePushed);
            pressurePlate.OnPressurePlateRelease.AddListener(PressurePlateRelease);
        }
    }

    private void PressurePlatePushed()
    {
        _playerCount++;
        if(_playerCount == 2)
            _onDoublePressurePlatePushed.Invoke();
    }

    private void PressurePlateRelease()
    {
        _playerCount--;
    }
}
