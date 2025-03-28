using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class InteractObject : ObjectBase, IInteractable
{
    [SerializeField] protected UnityEvent _interactEvents;

    [Button]
    public virtual void Interact()
    {
        _interactEvents.Invoke();
    }
}
