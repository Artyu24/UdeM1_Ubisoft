using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class GetPlayerForTargetGroup : MonoBehaviour
{

    [SerializeField] private CinemachineTargetGroup _targetGroup;
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.2f);

        foreach (var player in PlayerManager.instance._playerList)
        {
            _targetGroup.AddMember(player.transform, 1, 0);

        }
            
        
    }
    public void AddPLayerToTargetGroup(PlayerInput playerInput)
    {
        _targetGroup.AddMember(playerInput.transform, 1, 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
