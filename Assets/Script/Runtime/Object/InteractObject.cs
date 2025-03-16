using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InteractObject : ObjectBase, IInteractable
{
    [SerializeField] private UnityEvent _interactEvents;

    [Button]
    public void Interact()
    {
        Debug.Log("Trigger Event");
        _interactEvents.Invoke();
    }
}
