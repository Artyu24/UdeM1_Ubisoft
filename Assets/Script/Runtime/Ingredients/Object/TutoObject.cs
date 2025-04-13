using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutoObject : GrabObject
{
    [Header("Tuto")] 
    [SerializeField] private GameObject _canvas;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Image _imageTuto;
    [SerializeField] private Sprite _tutoSprite;
    [SerializeField] private float _fadeTime = 0.3f;
    
    private void Awake()
    {
        _imageTuto.sprite = _tutoSprite;
        _canvasGroup.alpha = 0;
    }

    public override bool OnGrab(Transform catcher)
    {
        bool isGrab = base.OnGrab(catcher);
        
        PlayerManager.instance.FreezePlayers();
        
        _canvas.SetActive(true);
        _canvasGroup.DOFade(1f, _fadeTime);
        
        return isGrab;
    }

    public override void OnRelease()
    {
        base.OnRelease();
        
        PlayerManager.instance.UnFreezePlayers();

        _canvasGroup.DOFade(0f, _fadeTime).OnComplete(() => _canvas.SetActive(false));
    }
}
