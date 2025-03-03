using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InteractObject : ObjectBase, IInteractable
{
    [SerializeField] private UnityEvent _interactEvents;
    
    public void Interact()
    {
        Debug.Log("Trigger Event");
        _interactEvents.Invoke();
    }
}
