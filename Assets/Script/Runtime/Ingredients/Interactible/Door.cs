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
    
    private int _playerCount;
    
    public void OpenDoor()
    {
        _playerCount++;
        
        if(_playerCount > 1)
            return;
        
        if(AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.SFX_PORTE);
        
        if (_isSlidingDoor)
            transform.DOLocalMove(_finalPosition, _doorAnimTime);
        else
            transform.DOLocalRotate(_finalRotation, _doorAnimTime);
    }
    
    public void CloseDoor()
    {
        _playerCount--;
        
        if(_playerCount > 0)
            return;
        
        if(AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.SFX_PORTE);
        
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
