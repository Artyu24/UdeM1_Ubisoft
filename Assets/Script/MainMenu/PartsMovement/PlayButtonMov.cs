using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class PlayButtonMov : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] float distance = 50f;
    [SerializeField] float duration = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        Sequence PlayMov = DOTween.Sequence();
        PlayMov.Append(rectTransform.DOAnchorPos(rectTransform.anchoredPosition + Vector2.down * distance, duration)
                                    .SetEase(Ease.InOutBack)
                                    .SetLoops(-1,LoopType.Yoyo));
        
    }
}
