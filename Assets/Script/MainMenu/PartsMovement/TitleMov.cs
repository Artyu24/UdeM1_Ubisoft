using DG.Tweening;
using UnityEngine;

public class TitleMov : MonoBehaviour
{
    private RectTransform rectTransform;

    [SerializeField] private float dropDistance = 600f;
    [SerializeField] private float dropTime = 0.6f;
    [SerializeField] private Ease dropEase = Ease.OutBounce;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        Vector2 finalPos = rectTransform.anchoredPosition;

        rectTransform.anchoredPosition = finalPos + Vector2.up * dropDistance;

        rectTransform.DOAnchorPos(finalPos, dropTime).SetEase(dropEase);
    }

}
