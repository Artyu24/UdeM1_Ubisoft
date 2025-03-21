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
    Normal,
    Agressive,
    Fear
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
public class AIScript : MonoBehaviour
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
    [Header("Player Reactions")]
    [SerializeField] ReactionToplayer _reactionToPlayer;
    [SerializeField] private bool _isChasingPlayer=false;
    Vector3 _lastPlayerSeenPosition = Vector3.zero;
    [field:SerializeField]public List<GameObject> interest { get; set; }
    private Coroutine _pushPlayer;


    public UnityEvent Onflee;
    public UnityEvent OnReachDestination;
    public UnityEvent OnReachReactionDestination;
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

        switch (_reactionToPlayer)
        {
            case ReactionToplayer.Push:
                if (_pushPlayer != null) return;//a move
                _pushPlayer = StartCoroutine(PushPlayer(player));
                break;
            case ReactionToplayer.Chase:
                break;
            default:
                break;
        }
    }
    private IEnumerator PushPlayer(PlayerMovement playerMovement)
    {
        playerMovement.OnIAPush(transform.parent.position);
        yield return new WaitForSeconds(1f);
        _pushPlayer = null;

    }
    private void ChasePlayer(PlayerMovement playerMovement)
    {

    }
    private IEnumerator ChaseStates(PlayerMovement playerMovement)
    {
        bool Chasing = true;
        _lastPlayerSeenPosition = playerMovement.transform.position;
        while (Chasing) 
        {
            _agent.SetDestination(_lastPlayerSeenPosition);
            if (LineOfSight.PlayerInSight.Contains(playerMovement))
            {
                _lastPlayerSeenPosition = playerMovement.transform.position;
                //if reach player
                //catch it and bring him outside the the area by throwing it
            }
            else 
            {
                yield return new WaitForSeconds(1.0f);
                Wander();
            }
            
        }
        yield return new WaitForSeconds(1.0f);


    }
}
