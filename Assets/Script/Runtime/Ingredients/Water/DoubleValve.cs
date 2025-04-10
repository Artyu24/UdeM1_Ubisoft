using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;

public class DoubleValve : MonoBehaviour
{
    [Header("Valve")]
    [SerializeField] private Valve _firstValve;
    [SerializeField] private Valve _secondValve;

    [Header("Interaction")]
    [SerializeField] private bool _mustDropWater;
    
    [SerializeField] private float _cooldown = 0.5f;
    [SerializeField] private UnityEvent _OnDoubleInteractionEvent;

    private bool _isAlreadyCall;
    
    void Start()
    {
        _firstValve.SetDoubleMode(InteractionCallback);
        _secondValve.SetDoubleMode(InteractionCallback);
    }

    private void InteractionCallback(SplineAnimate splineAnimate)
    {
        if (!_isAlreadyCall)
        {
            _isAlreadyCall = true;
            StartCoroutine(InteractionCooldown());
        }
        else
        {
            splineAnimate.Completed += () =>
            {
                if (_OnDoubleInteractionEvent != null)
                {
                    _OnDoubleInteractionEvent.Invoke();
                }
                
                if (_firstValve != null)
                {
                    if(_firstValve.Pipe.DropWaterPipe != null)
                        _firstValve.Pipe.DropWaterPipe.DropWaterBelow(_mustDropWater);
                }
            };
        }
    }

    private IEnumerator InteractionCooldown()
    {
        yield return new WaitForSeconds(_cooldown);
        _isAlreadyCall = false;
    }
}
