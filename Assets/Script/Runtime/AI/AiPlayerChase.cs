using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;



public enum ChaseState
{
    none,
    Chasing,
    LookingArround,
    Stunned,
    HoldPlayer,
    yeetPlayer,
    Leaving

}
public class AiPlayerChase : AIBehavior
{
    [Header("grab")]
    [SerializeField] Transform _grabPosition;
    [SerializeField] float _StunTime = 3f;
    [SerializeField] Transform _DropPosition;
    private PlayerMovement _grabbedPlayer;



    Vector3 _lastPlayerSeenPosition = Vector3.zero;


    private PlayerMovement _currentChasedPlayer;
    [SerializeField] ChaseState _chaseState;
    void Update()
    {
        AiBrain.State = AiBrain.State;
        switch (AiBrain.State)
        {
            case npcState.chasing:
                ChasePlayer(_currentChasedPlayer);
                break;
            case npcState.Stunned:
                AiBrain._agent.isStopped=true;
                break;

            default:
                break;
        }
    }
    private void EjectPLayer()
    {
        if (_grabbedPlayer != null)
        {
            StartCoroutine(IgnorePlayer());
            _grabbedPlayer.SetIsGrabed(false);
            _grabbedPlayer.transform.parent = null;
            _grabbedPlayer.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
            DontDestroyOnLoad(_grabbedPlayer);
            _grabbedPlayer = null;
            if (AiBrain.State == npcState.Stunned) return;
            AiBrain.State = npcState.wandering;
        }
    }
    private void ChasePlayer(PlayerMovement playerMovement)
    {
        if(_chaseState!= ChaseState.HoldPlayer)
        AiBrain.SetDestination(_lastPlayerSeenPosition);
        if (AiBrain.LineOfSight.PlayerInSight.Contains(playerMovement))
        {
            _lastPlayerSeenPosition = playerMovement.transform.position;

        }

    }
    public override void ReactToPlayer(PlayerMovement player)
    {
        if (AiBrain.State == npcState.chasing || _isGnoringPlayer|| AiBrain.State == npcState.Stunned) return;
        if (player == null) return;
        if (_reactionCoroutine != null) return;//a move

        InitChase(player);

        
    }
    private void InitChase(PlayerMovement playerMovement)
    {
        AiBrain.State = npcState.chasing;
        if (_lookingArroundCorou != null)
        {
            InterruptCorouAction(_lookingArroundCorou);
        }
        _currentChasedPlayer = playerMovement;
        _lastPlayerSeenPosition = playerMovement.transform.position;
        ChasePlayer(playerMovement);
    }
    public override void ReachAIDestination()
    {
        base.ReachAIDestination();
        switch (_chaseState)
        {

            case ChaseState.Chasing:
                if (_lookingArroundCorou == null)
                    _lookingArroundCorou = StartCoroutine(LookArround());
                break;
            case ChaseState.LookingArround:
                break;
            case ChaseState.Stunned:
                break;
            case ChaseState.Leaving:
                break;
            case ChaseState.HoldPlayer:

                _chaseState = ChaseState.yeetPlayer;


                break;
            case ChaseState.yeetPlayer:
                EjectPLayer();
                if (_lookingArroundCorou != null)
                {
                    StartCoroutine(DelayBeforeWandering());
                }
                break;
            default:
                break;
        }
    }
    public void GoToPlayerReleasePoint()
    {
        _isGnoringPlayer = true;
        AiBrain.SetDestination(_DropPosition.position);
        
    }
    [Button]
    public override void Interact()
    {
        //OnHited.Invoke();
        if (_grabbedPlayer != null)
        {
            EjectPLayer();
            StartCoroutine(StunnedDelay());
            _chaseState=ChaseState.none;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement) && _grabbedPlayer == null)
        {
            if (AiBrain.State != npcState.chasing) return;
            _grabbedPlayer = playerMovement;
            playerMovement.SetIsGrabed(true);
            playerMovement.transform.position = _grabPosition.position;
            playerMovement.gameObject.transform.SetParent(transform);                   
            _chaseState = ChaseState.HoldPlayer;
            GoToPlayerReleasePoint();
        }
    }
}
