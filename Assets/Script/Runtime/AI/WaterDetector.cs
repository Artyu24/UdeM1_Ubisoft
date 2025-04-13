using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnWaterDetected(WaterPuddle wp);

public class WaterDetector : MonoBehaviour
{
    [SerializeField,Required] private AICleaner _cleaner;

    OnWaterDetected onWaterDetected;
    
    private List<WaterPuddle> _currentPuddle = new List<WaterPuddle>();
    private void OnTriggerStay(Collider other)
    {
        if(!other.gameObject.TryGetComponent<WaterPuddle>(out WaterPuddle wp)) return;
        if(!_currentPuddle.Contains(wp))
        {
            _currentPuddle.Add(wp);
            onWaterDetected.Invoke(wp);
            wp.onWaterDry += _cleaner.OncompleteCleaning;
        }
    }
    void Start()
    {
        onWaterDetected += _cleaner.AddWater;
    }
}
