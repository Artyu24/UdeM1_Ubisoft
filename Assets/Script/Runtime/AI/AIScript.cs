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
    runningAway
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

    public npcState State { get; protected set; }

    [Header("Player Reactions")]
    [SerializeField] ReactionToplayer _reactionToPlayer;
    [SerializeField] private bool _isChasingPlayer=false;
    Vector3 _lastPlayerSeenPosition = Vector3.zero;
    [field:SerializeField]public List<GameObject> interest { get; set; }
    private Coroutine _reactionCoroutine;
    private Sequence sequence = DOTween.Sequence();

    [Header("grab")]
    [SerializeField] Transform _grabPosition;
    private PlayerMovement _grabbedPlayer;

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

        AIEventHandler.instance.Ai.Add(this);
    }
    void Update()
    {
        if (!_agent.pathPending)//wandering loop, [refacto possible]
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    if (!_isRunningAway && _wandererDelay==null)
                    {
                        OnReachDestination.Invoke();
                        transform.DORotate(_guardRotation,0.5f);
                        _wandererDelay = StartCoroutine(DelayBeforeWandering());
                    }
                    else if (_isRunningAway)
                    {
                        OnReachReactionDestination.Invoke();
                        LookForObject(_objectType);
                        
                    }
                }
            }
        }
    }
    [Button]
    public void fleeToPoint()
    {
        _agent.destination = _reactionZone.position;
        _isRunningAway=true;
        StopAllCoroutines();
    }

    public void LookForObject(string objectType)
    {
        AIObject aIObject = _lineOfSight.GetSightObjectByType(objectType);
        if (aIObject == null)
            LeaveZone();
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
    public void Wander()
    {
        _agent.destination = _wanderPoints[Random.Range(0,_wanderPoints.Count-1)].position;
        
    }
    IEnumerator DelayBeforeWandering()
    {
        yield return new WaitForSeconds(WanderCoolDownTime);
        Wander();
        _wandererDelay=null;
    }

    public void ReactoPlayer(PlayerMovement player)
    {
        if (player == null) return;
        if (_reactionCoroutine != null) return;//a move
        switch (_reactionToPlayer)
        {
            case ReactionToplayer.Push:
                _reactionCoroutine = StartCoroutine(PushPlayer(player));
                break;
            case ReactionToplayer.Chase:
                sequence.Kill();
                _reactionCoroutine = StartCoroutine(ChaseStates(player));
                break;
            default:
                break;
        }
    }
    private IEnumerator PushPlayer(PlayerMovement playerMovement)
    {
        playerMovement.OnIAPush(transform.parent.position);
        yield return new WaitForSeconds(1f);
        _reactionCoroutine = null;

    }
    private void ChasePlayer(PlayerMovement playerMovement)
    {

    }
    private IEnumerator ChaseStates(PlayerMovement playerMovement)
    {
        State = npcState.chasing;
        Debug.Log("start chase");
        bool Chasing = true;
        _lastPlayerSeenPosition = playerMovement.transform.position;
        while (Chasing) 
        {
            _agent.SetDestination(_lastPlayerSeenPosition);
            if (LineOfSight.PlayerInSight.Contains(playerMovement))
            {
                _lastPlayerSeenPosition = playerMovement.transform.position;
                yield return null;

                //if reach player
                //catch it and bring him outside the the area by throwing it
            }
            else 
            {

                Chasing = false;
                LookArround();

            }
        }
        _reactionCoroutine= null;
    }
    private void LookArround()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y+ 90, transform.rotation.z), 0.5f));
        sequence.AppendInterval(1f);
        sequence.Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y - 90, transform.rotation.z), 0.5f));
        sequence.AppendInterval(1f).OnComplete(() => { Debug.Log("comp"); Wander(); }); // a changer!
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement)&& _grabbedPlayer==null)
        {
            _grabbedPlayer = playerMovement;
            playerMovement.SetIsGrabed(true);
            playerMovement.transform.position= _grabPosition.position;
            playerMovement.gameObject.transform.SetParent(transform);
            
        }
    }
    [Button]
    public void Interact()
    {
        OnHited.Invoke();
        if (_grabbedPlayer != null) 
        {
            _grabbedPlayer.SetIsGrabed(false);
            _grabbedPlayer.transform.parent=null;

            _grabbedPlayer.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
        }
    }
}
