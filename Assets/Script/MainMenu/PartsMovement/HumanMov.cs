using DG.Tweening;
using UnityEngine;

public class HumanMov : MonoBehaviour
{
    private RectTransform rect;

    [SerializeField] private float moveDistance = 20f;
    [SerializeField] private float speed = 0.9f;
    [SerializeField] private float delay = 0.4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rect = GetComponent<RectTransform>();

        // movement
        rect.DOAnchorPosX(rect.anchoredPosition.x + moveDistance, speed)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(delay);

        // tilt
        rect.DORotate(new Vector3(0,0,5), 0.3f)
            .SetLoops(-1, LoopType .Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
