using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TutoManager : MonoBehaviour
{
    private List<PlayerData> _playerDataList = new List<PlayerData>();

    [Header("Components")] 
    [SerializeField] private TutoCanvas _tutoCanvas;
    [SerializeField, Scene] private string _hubSceneName;
    
    [Header("Point")]
    [SerializeField] private Transform _firstPlayerInitPos;
    [SerializeField] private Transform _firstPlayerEndPos;
    [SerializeField] private Transform _secondPlayerInitPos;
    [SerializeField] private Transform _secondPlayerEndPos;
    
    [Header("Move Speed")]
    [SerializeField] private float _speed = 2f;
    
    private int _numberOfPlayerReady;
    private Coroutine _lauchGameCoroutine;
    
    public void OnPlayerJoin(PlayerInput playerInput)
    {
        PlayerData pData = playerInput.gameObject.GetComponent<PlayerData>();
        if(DebugHelper.IsNull(pData, name, nameof(PlayerManager)))
            return;

        if(pData.PlayerIndex > 1)
            return;
        
        _playerDataList.Add(pData);
        
        if (pData.PlayerIndex == 0)
        {
            PlayerFirstAction(playerInput, pData, _firstPlayerInitPos, _firstPlayerEndPos, new Vector3(-0.5f, 0, 0.5f));
        }
        else if (pData.PlayerIndex == 1)
        {
            PlayerFirstAction(playerInput, pData, _secondPlayerInitPos, _secondPlayerEndPos, new Vector3(0.5f, 0, 0.5f));
        }
    }

    private void PlayerFirstAction(PlayerInput playerInput, PlayerData pData, Transform initPos, Transform endPos, Vector3 lookRot)
    {
        pData.IsInTuto = true;
            
        pData.AnimController.SetFloat("Speed", 1);
            
        playerInput.transform.position = initPos.position;
        pData.PlayerMesh.rotation = Quaternion.LookRotation(lookRot , Vector3.up);
            
        playerInput.transform.DOMove(endPos.position, _speed).OnComplete(() =>
        {
            _tutoCanvas.AppearReadyText(pData.PlayerIndex);
            
            pData.AnimController.SetFloat("Speed", 0);
            
            pData.PTutorial.OnPlayerAcceptAction += () => SetReady(pData);
        });
    }

    private void SetReady(PlayerData pData)
    {
        pData.IsReadyToPlay = !pData.IsReadyToPlay;
        pData.AnimController.applyRootMotion = pData.IsReadyToPlay;
        pData.AnimController.SetBool("IsReady", pData.IsReadyToPlay);

        _tutoCanvas.SwitchText(pData.PlayerIndex, pData.IsReadyToPlay);
        
        if (pData.IsReadyToPlay)
            _numberOfPlayerReady++;
        else
            _numberOfPlayerReady--;

        if (_numberOfPlayerReady == 2)
            _lauchGameCoroutine = StartCoroutine(LaunchTutorial());
        else if (_lauchGameCoroutine != null)
        {
            StopCoroutine(_lauchGameCoroutine);
            _lauchGameCoroutine = null;
        }
    }

    private IEnumerator LaunchTutorial()
    {
        yield return new WaitForSeconds(2f);
        foreach (PlayerData pData in _playerDataList)
        {
            pData.AnimController.applyRootMotion = false;
            pData.PTutorial.OnPlayerAcceptAction = null;
            pData.AnimController.SetBool("IsReady", false);
        }
        
        _tutoCanvas.HideReadyText();
        
        HUDManager.instance.FadeInTransition(ShowBD);
    }

    private void ShowBD()
    {
        _tutoCanvas.ShowBD(() =>
        {
            foreach (PlayerData pData in _playerDataList)
            {
                pData.PTutorial.OnPlayerAcceptAction += LaunchGame;
            }
        });
    }

    private void LaunchGame()
    {
        foreach (PlayerData pData in _playerDataList)
        {
            pData.IsInTuto = false;
            pData.PTutorial.OnPlayerAcceptAction = null;
        }

        _tutoCanvas.BdOffset.DOFade(0f, 0.5f).OnComplete(() => SceneManager.LoadScene(_hubSceneName));
    }
}
