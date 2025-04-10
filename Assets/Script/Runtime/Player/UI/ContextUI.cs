using System;
using UnityEngine;
using UnityEngine.UI;

public class ContextUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Sprite _textureB;
    [SerializeField] private Sprite _textureX;
    private bool interactionContextOn; 
    
    [Header("Component")] 
    [SerializeField] private Image _image;
    [SerializeField] private PlayerInteraction _playerInteraction;
    
    private void OnTriggerStay(Collider other)
    {
        if (!_playerInteraction.CanInteract && !_playerInteraction.DoPlayerInteractActionPossible)
        {
            HideContextUI();
            return;
        }
        
        if (other.GetComponent<IInteractable>() != null || _playerInteraction.DoPlayerInteractActionPossible)
        {
            _image.sprite = _textureX;
            _image.enabled = true;
            
            interactionContextOn = true;
        }
        else if (other.GetComponent<IGrabbable>() != null && !interactionContextOn)
        {
            _image.sprite = _textureB;
            _image.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null || other.GetComponent<IGrabbable>() != null)
            HideContextUI();
    }

    private void HideContextUI()
    {
        _image.enabled = false;

        interactionContextOn = false;
    }
}
