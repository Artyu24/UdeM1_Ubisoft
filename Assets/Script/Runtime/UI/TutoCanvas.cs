using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutoCanvas : MonoBehaviour
{
    [Header("Text")] 
    [SerializeField] private TextMeshProUGUI _playerOneText;
    [SerializeField] private TextMeshProUGUI _playerTwoText;
    private string _pressAString = "Press <color=#718E1D>A</color> to be <color=#718E1D>Ready</color>";
    
    [Header("BD")]
    [SerializeField] private CanvasGroup _bdOffset;
    public CanvasGroup BdOffset => _bdOffset;
    [SerializeField] private Image _first, _second, _third, _fourth, _fifth, _sixth, _buttonA;
    
    [Header("Anim")]
    private float _fadeValue = 1f;
    [SerializeField] private float _fadeSpeed = 2f;
    [SerializeField] private float _delay = 2f;

    public void AppearReadyText(int playerIndex)
    {
        if (playerIndex == 0)
        {
            _playerOneText.text = _pressAString;
            _playerOneText.DOFade(1f, 1f);
        }
        else if (playerIndex == 1)
        {
            _playerTwoText.text = _pressAString;
            _playerTwoText.DOFade(1f, 1f);
        }
    }
    
    public void SwitchText(int playerIndex, bool isReady)
    {
        string text = _pressAString;
        if (isReady)
        {
            string color = "#63D2FF";
            string name = "Rocco";
            if (playerIndex == 1)
            {
                color = "#FF7151";
                name = "Munch";
            }
            
            text = "<color=" + color + ">" + name + "</color> is <color=#718E1D>Ready</color>";
        }

        if (playerIndex == 0)
            _playerOneText.text = text;
        else if (playerIndex == 1)
            _playerTwoText.text = text;
    }

    public void HideReadyText()
    {
        _playerOneText.DOFade(0f, 1f);
        _playerTwoText.DOFade(0f, 1f);
    }
    
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
