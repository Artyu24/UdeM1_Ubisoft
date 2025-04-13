using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTutorial : MonoBehaviour
{
    public Action OnPlayerAcceptAction;

    private bool _isInCD;
    
    public void OnPlayerAccept(InputAction.CallbackContext ctx)
    {
        if(!ctx.started || _isInCD)
            return;

        if (OnPlayerAcceptAction != null)
        {
            OnPlayerAcceptAction.Invoke();
            StartCoroutine(AcceptCD());
        }
    }

    private IEnumerator AcceptCD()
    {
        _isInCD = true;
        yield return new WaitForSeconds(2f);
        _isInCD = false;
    }
}
