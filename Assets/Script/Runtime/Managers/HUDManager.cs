using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;

    [SerializeField] private RectTransform _transitionEffect;
    [SerializeField] private float _duration;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        _transitionEffect.localPosition = new Vector3(0, _transitionEffect.localPosition.y, _transitionEffect.localPosition.z);
        FadeOutTransition();
        
        SceneManager.sceneLoaded += (arg0, mode) => FadeOutTransition();
    }

    public void FadeInTransition(Action onFadeOutComplete)
    {
        _transitionEffect.DOLocalMoveX(0, _duration).SetEase(Ease.OutQuint).OnComplete(() => onFadeOutComplete());
    }

    private void FadeOutTransition()
    {
        _transitionEffect.DOLocalMoveX(-2400f, _duration).SetEase(Ease.OutQuint);
    }
}
