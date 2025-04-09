using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class SlideSign : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public UnityEvent Ondisappear;
    [SerializeField] float _disaperAnimationDuration=1f;

    public void disappear()
    {
        transform.DOScale(Vector3.zero, _disaperAnimationDuration).SetEase(Ease.OutBounce).OnComplete(() => Destroy(gameObject));
        Ondisappear.Invoke();

    }
}
