using System;
using UnityEngine;
using UnityEngine.Splines;

public class PipePathEffect : MonoBehaviour
{
    [SerializeField] private SplineAnimate _splineAnimate;
    [SerializeField] private ParticleSystem _particleSystem;
    
    private bool _canBeDestroy;
    
    private void Start()
    {
        if(AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.SFX_EAU_QUI_TRAVERSE_LE_TUYAUX);
        
        _splineAnimate.Completed += () => { _canBeDestroy = true; };
    }

    private void Update()
    {
        if(!_canBeDestroy)
            return;
        
        _particleSystem.Stop();
        if(_particleSystem.particleCount == 0)
            Destroy(gameObject);
    }
}
