using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float _doorAnimTime = 0.5f;
    
    [SerializeField] private bool _isSlidingDoor;
    
    [HideIf("_isSlidingDoor"), SerializeField] private Vector3 _defaultRotation;
    [HideIf("_isSlidingDoor"), SerializeField] private Vector3 _finalRotation;
    
    [ShowIf("_isSlidingDoor"), SerializeField] private Vector3 _defaultPosition;
    [ShowIf("_isSlidingDoor"), SerializeField] private Vector3 _finalPosition;
    
    public void OpenDoor()
    {
        if (_isSlidingDoor)
            transform.DOLocalMove(_finalPosition, _doorAnimTime);
        else
            transform.DOLocalRotate(_finalRotation, _doorAnimTime);
    }
    
    public void CloseDoor()
    {
        if (_isSlidingDoor)
            transform.DOLocalMove(_defaultPosition, _doorAnimTime);
        else
            transform.DOLocalRotate(_defaultRotation, _doorAnimTime);
    }

#if UNITY_EDITOR
    [Button]
    private void EditorOpenDoor()
    {
        if (_isSlidingDoor)
            transform.localPosition = _finalPosition;
        else
            transform.localEulerAngles = _finalRotation;
    }
    
    [Button]
    private void ResetDoor()
    {
        if (_isSlidingDoor)
            transform.localPosition = _defaultPosition;
        else
            transform.localEulerAngles = _defaultRotation;
    }
#endif
}
