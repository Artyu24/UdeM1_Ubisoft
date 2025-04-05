using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Splines;

public class Valve : InteractObject
{
    [Header("Simple Pipe")]
    [SerializeField] private Transform _valve;
    [SerializeField] private PipeTool _pipeTool;
    
    [Header("Double Pipe Mod")]
    public PipeTool Pipe => _pipeTool;
    private bool _isInDoubleMode;
    private Action<SplineAnimate> _doubleModeCallback;
    
    public void SetDoubleMode(Action<SplineAnimate> callback)
    {
        _isInDoubleMode = true;
        _doubleModeCallback += callback;
    }
    
    public override void Interact()
    {
        if (_valve != null)
            _valve.DOLocalRotate(new Vector3(_valve.localEulerAngles.x, _valve.localEulerAngles.y, _valve.localEulerAngles.z + 170), 1f);

        //If Valve is in Simple Mode, play Interact Event as Usual
        if (_pipeTool != null)
        {
            SplineAnimate anim = _pipeTool.PipeEffect();
         
            //If Valve is in Double Mode, dont play the Interact event of the valve
            if (!_isInDoubleMode)
            {
                anim.Completed += () =>
                {
                    _interactEvents.Invoke();
                    _pipeTool.DropWaterPipe.DropWaterBelow();
                };
            }
            
            //If double mode, play callback
            if(_isInDoubleMode && _doubleModeCallback != null)
                _doubleModeCallback.Invoke(anim);
        }
    }
}
