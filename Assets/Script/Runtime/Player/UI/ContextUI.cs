using System;
using UnityEngine;
using UnityEngine.UI;

public class ContextUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Sprite _textureB;
    [SerializeField] private Sprite _textureX;
    private bool _interactionContextOn;
    private bool _doPlayerInteractActionMem;
    
    [Header("Component")] 
    [SerializeField] private Image _image;
    [SerializeField] private PlayerInteraction _playerInteraction;
    
    private void OnTriggerStay(Collider other)
    {
        if (!_playerInteraction.CanInteract && !_playerInteraction.DoPlayerInteractActionPossible ||
            !_playerInteraction.DoPlayerInteractActionPossible && _doPlayerInteractActionMem)
        {
            HideContextUI();
            return;
        }
        
        if (other.GetComponent<IInteractable>() != null || _playerInteraction.DoPlayerInteractActionPossible)
        {
            _image.sprite = _textureB;
            _image.enabled = true;
            
            _interactionContextOn = true;
            _doPlayerInteractActionMem = _playerInteraction.DoPlayerInteractActionPossible;
        }
        else if (other.GetComponent<IGrabbable>() != null && !_interactionContextOn)
        {
            _image.sprite = _textureX;
            _image.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IInteractable>() != null || other.GetComponent<IGrabbable>() != null || other.GetComponent<TeleportPlayers>() != null)
            HideContextUI();
    }

    private void HideContextUI()
    {
        _image.enabled = false;

        _interactionContextOn = false;
        _doPlayerInteractActionMem = false;
    }
}
