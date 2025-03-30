using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class Valve : InteractObject
{
    [SerializeField] private Transform _valve;
    [SerializeField] private PipeTool _pipeTool;
    
    public override void Interact()
    {
        if (_valve != null)
            _valve.DOLocalRotate(new Vector3(_valve.localEulerAngles.x, _valve.localEulerAngles.y, _valve.localEulerAngles.z + 170), 1f);

        if (_pipeTool != null)
        {
            SplineAnimate anim = _pipeTool.PipeEffect();
            anim.Completed += () => { _interactEvents.Invoke(); };
        }
    }
}
