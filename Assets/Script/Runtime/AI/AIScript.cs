using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Rendering;

public enum npcState
{
    wandering,
    chasing,
    Reacting,
    Stunned,
    leaving,
    HoldPlayer,
    LookingArround,
    yeetPlayer
}
public enum npcType
{
    notmoving,
    wander,
    cleaner
}

public enum ReactionToplayer
{
    Push,
    Chase,
}
public class AIScript : MonoBehaviour, IInteractable
{
    private float anger;
    private float fear;

    [Header("Wander")]
    [SerializeField] private Vector3 _guardRotation;
    [SerializeField] private List<Transform> _wanderPoints = new List<Transform>();
    [SerializeField] private float WanderCoolDownTime = 1f;
    private Coroutine _wandererDelay;
    [Header("Running away")]
    [SerializeField] private Transform _reactionZone;
    [SerializeField] private Transform _leavePoint;
    [SerializeField] private string _objectType;
    [SerializeField] private bool _isRunningAway;

    [Header("AI INFO")]
    [SerializeField, Required] private LineOfSight _lineOfSight;
    [SerializeField] private LayerMask _viewMak;
    [SerializeField, Required] private NavMeshAgent _agent;
    public LineOfSight LineOfSight { get; set; }//auto ref

    [field:SerializeField]public npcState State { get; protected set; }

    [Header("Player Reactions")]
    [SerializeField] ReactionToplayer _reactionToPlayer;
    [SerializeField] private bool _isChasingPlayer=false;
    Vector3 _lastPlayerSeenPosition = Vector3.zero;
    [field:SerializeField]public List<GameObject> interest { get; set; }
    private Coroutine _reactionCoroutine;
    private Coroutine _lookingArroundCorou;

    [Header("Player Reactions")]
    private PlayerMovement _currentChasedPlayer;

    [Header("grab")]
    [SerializeField] Transform _grabPosition;
    [SerializeField] float _StunTime = 3f;
    [SerializeField] Transform _DropPosition;
    private PlayerMovement _grabbedPlayer;
    bool _isGnoringPlayer = false;

    public UnityEvent Onflee;
    public UnityEvent OnReachDestination;
    public UnityEvent OnReachReactionDestination;
    public UnityEvent OnHited;
    private void OnDrawGizmosSelected()
    {
    
    }
    void Start()
    {
        Wander();
        _agent.destination = _wanderPoints[Random.Range(0,_wanderPoints.Count-1)].position;
        AIEventHandler.instance.Ai.Add(this);
    }
    void Update()
    {
        if (!_agent.pathPending)//check is succes to go to destination
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    ReachAIDestination();
                }
            }
        }

    }
    void LateUpdate()
    {
        StateUpdate();
    }
    private void StateUpdate()
    {
        switch (State)
        {
            case npcState.chasing:
                ChasePlayer(_currentChasedPlayer);
                break;
            

            default:
                break;
        }
    }
    private void ReachAIDestination()
    {
        switch (State)
        {
            case npcState.wandering:
                if (_wandererDelay == null)//no
                {
                    OnReachDestination.Invoke();
                    transform.DORotate(_guardRotation, 0.5f);
                    _wandererDelay = StartCoroutine(DelayBeforeWandering());
                }
                break;
            case npcState.chasing:
                if (_lookingArroundCorou == null)
                    _lookingArroundCorou = StartCoroutine(LookArround());
                break;
            case npcState.LookingArround:
                break;

            case npcState.Reacting:
                OnReachReactionDestination.Invoke();
                LookForObject(_objectType);
                break;
            case npcState.Stunned:
                break;
            case npcState.leaving:
                break;
            case npcState.HoldPlayer:
                GoToTrash();
                break;
            case npcState.yeetPlayer:
                EjectPLayer();
                StartCoroutine(DelayBeforeWandering());
                break;
            default:
                break;
        }
    }
    [Button]
    public void fleeToPoint()
    {
        _agent.destination = _reactionZone.position;
        State= npcState.leaving;
        _isRunningAway=true;
        StopAllCoroutines();// kill all action running
    }

    public void LookForObject(string objectType)
    {
        AIObject aIObject = _lineOfSight.GetSightObjectByType(objectType);
        if (aIObject == null)
        {
            State = npcState.leaving;
            LeaveZone();
        }
        else
            UseObject(aIObject);
    }
    public void UseObject(AIObject ob)
    {
        StartCoroutine(ItemUseDelay());
        IEnumerator ItemUseDelay()
        {
            yield return new WaitForSeconds(ob.useTime);
            Wander();
            _isRunningAway = false;
        }
    }
    public void LeaveZone()
    {
        _agent.destination = _leavePoint.position;
    }
    public void GoToTrash()
    {
        _agent.destination = _DropPosition.position;
        State = npcState.yeetPlayer;
    }
    public void Wander()
    {
        _agent.destination = _wanderPoints[Random.Range(0,_wanderPoints.Count-1)].position; 
    }

    public void ReactoPlayer(PlayerMovement player)
    {
        if(State == npcState.HoldPlayer || State == npcState.chasing|| _isGnoringPlayer) return;
        if (player == null) return;
        if (_reactionCoroutine != null) return;//a move
        switch (_reactionToPlayer)
        {
            case ReactionToplayer.Push:
                _reactionCoroutine = StartCoroutine(PushPlayer(player));
                break;
            case ReactionToplayer.Chase:
                InitChase(player);
                //_reactionCoroutine = StartCoroutine(ChaseStates(player));
                break;
            default:
                break;
        }
    }

    private void ChasePlayer(PlayerMovement playerMovement)
    {
        _agent.SetDestination(_lastPlayerSeenPosition);
        if (LineOfSight.PlayerInSight.Contains(playerMovement))
        {
            _lastPlayerSeenPosition = playerMovement.transform.position;
            
        }

    }
    private void InitChase(PlayerMovement playerMovement)
    {
        State = npcState.chasing;
        if (_lookingArroundCorou != null)
        {
            InterruptCorouAction(_lookingArroundCorou);
        }
        _currentChasedPlayer = playerMovement;
        _lastPlayerSeenPosition = playerMovement.transform.position;
        ChasePlayer(playerMovement);
    }
    private void InterruptCorouAction(Coroutine corouAction) 
    {
        if (corouAction == null) return;
        StopCoroutine(corouAction);
        corouAction=null;
    }
    private IEnumerator LookArround()
    {
        
        State= npcState.LookingArround;
        transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y+ 90, transform.rotation.z), 0.5f);
        yield return new WaitForSeconds(1f);
        transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y - 90, transform.rotation.z), 0.5f);
        yield return new WaitForSeconds(1f);;

        
        State = npcState.wandering;
        _lookingArroundCorou = null;

        Wander();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement)&& _grabbedPlayer==null)
        {
            if(State!=npcState.chasing) return;
            _grabbedPlayer = playerMovement;
            playerMovement.SetIsGrabed(true);
            playerMovement.transform.position= _grabPosition.position;
            playerMovement.gameObject.transform.SetParent(transform);
            State = npcState.HoldPlayer;
            GoToTrash();
                  }
    }
    [Button]
    public void Interact()
    {
        OnHited.Invoke();
        if (_grabbedPlayer != null)
        {
            EjectPLayer();
            StartCoroutine(StunnedDelay());
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
            if(State==npcState.Stunned) return;
            State = npcState.wandering;
        }
    }
    private IEnumerator StunnedDelay()
    {
        State= npcState.Stunned;
        yield return new WaitForSeconds(_StunTime);
        State = npcState.wandering;
    }
    private IEnumerator PushPlayer(PlayerMovement playerMovement)
    {
        playerMovement.OnIAPush(transform.parent.position);
        yield return new WaitForSeconds(1f);
        _reactionCoroutine = null;

    }
    IEnumerator DelayBeforeWandering()
    {
        yield return new WaitForSeconds(WanderCoolDownTime);
        Wander();
        _wandererDelay = null;
    }
    IEnumerator IgnorePlayer()
    {
        _isGnoringPlayer = true;
        yield return new WaitForSeconds(1);
    }

}
