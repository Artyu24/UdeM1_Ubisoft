using DG.Tweening;
using UnityEngine;

public class Valve : InteractObject
{
    [SerializeField] private Transform _valve;
    
    public override void Interact()
    {
        base.Interact();

        if (_valve != null)
            _valve.DOLocalRotate(new Vector3(_valve.localEulerAngles.x, _valve.localEulerAngles.y, _valve.localEulerAngles.z + 170), 1f);
    }
}
