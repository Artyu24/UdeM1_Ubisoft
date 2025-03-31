using System;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTriggerEventRef : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider> OnTriggerEnterEvent;
    [SerializeField] private UnityEvent<Collider> OnTriggerExitEvent;
    
    private void OnTriggerEnter(Collider other) => OnTriggerEnterEvent.Invoke(other);
    private void OnTriggerExit(Collider other) => OnTriggerExitEvent.Invoke(other);
}