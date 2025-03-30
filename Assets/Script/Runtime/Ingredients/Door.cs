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
            transform.DOMove(_finalPosition, _doorAnimTime);
        else
            transform.DORotate(_finalRotation, _doorAnimTime);
    }
    
    public void CloseDoor()
    {
        if (_isSlidingDoor)
            transform.DOMove(_defaultPosition, _doorAnimTime);
        else
            transform.DORotate(_defaultRotation, _doorAnimTime);
    }

#if UNITY_EDITOR
    [Button]
    private void EditorOpenDoor()
    {
        if (_isSlidingDoor)
            transform.position = _finalPosition;
        else
            transform.eulerAngles = _finalRotation;
    }
    
    [Button]
    private void ResetDoor()
    {
        if (_isSlidingDoor)
            transform.position = _defaultPosition;
        else
            transform.eulerAngles = _defaultRotation;
    }
#endif
}
