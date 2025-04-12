using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TutoCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup _bdOffset;
    public CanvasGroup BdOffset => _bdOffset;
    [SerializeField] private Image _first, _second, _third, _fourth, _fifth, _sixth, _buttonA;
    
    private float _fadeValue = 1f;
    [SerializeField] private float _fadeSpeed = 2f;
    [SerializeField] private float _delay = 2f;


    public void ShowBD(Action endTutoAction)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_first.DOFade(_fadeValue, _fadeSpeed));
        seq.Append(_second.DOFade(_fadeValue, _fadeSpeed)).SetDelay(_delay, false);
        seq.Append(_third.DOFade(_fadeValue, _fadeSpeed)).SetDelay(_delay, false);
        seq.Append(_fourth.DOFade(_fadeValue, _fadeSpeed)).SetDelay(_delay, false);
        seq.Append(_fifth.DOFade(_fadeValue, _fadeSpeed)).SetDelay(_delay, false);
        seq.Append(_sixth.DOFade(_fadeValue, _fadeSpeed)).SetDelay(_delay, false);
        seq.Append(_buttonA.DOFade(_fadeValue, _fadeSpeed)).SetDelay(_delay, false).OnComplete(() => { endTutoAction.Invoke(); });
        seq.Play();
    }
}
