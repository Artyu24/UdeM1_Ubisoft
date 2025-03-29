using DG.Tweening;
using NaughtyAttributes;
using System;
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
public class AIScript : MonoBehaviour
{
    private float anger;
    private float fear;


    [Header("AI Data")]


    [Header("Wander")]
    [field: SerializeField] public List<Transform> WanderPoints { get; set; }
    [field: SerializeField] public float WanderCoolDownTime { get; set; } = 1f;



    [Header("AI INFO")]
    //[field:SerializeField, Required("AI Need a line of sight")] public LineOfSight LineOfSight { get; set; }
    [SerializeField] private LayerMask _viewMak;
    [field:SerializeField, Required] public NavMeshAgent _agent {  get; set; }
    public LineOfSight LineOfSight { get; set; }//auto ref

    [field: SerializeField] public npcState State { get; set; }

    [Header("Player Reactions")]
    [field: SerializeField] ReactionToplayer ReactionToPlayer { get; set; }
    Vector3 _lastPlayerSeenPosition = Vector3.zero;
    private Coroutine _reactionCoroutine;
    private Coroutine _lookingArroundCorou;

    [Header("Player Reactions")]


    [Header("grab")]
    [SerializeField] Transform _grabPosition;
    [field: SerializeField] public float StunTime { get; set; } = 3f;
    [SerializeField] Transform _DropPosition;
    private PlayerMovement _grabbedPlayer;
    bool _isGnoringPlayer = false;

    public UnityEvent Onflee;
    public UnityEvent OnReachDestination;
    public UnityEvent OnReachReactionDestination;
    public UnityEvent OnHited;


    [SerializeField] private AIBehavior _behavior;
    private void OnDrawGizmosSelected()
    {
    
    }
    void Start()
    {
        AIEventHandler.instance.Ai.Add(this);
    }
    void LateUpdate()
    {

        if (!_agent.pathPending)//check is succes to go to destination
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    _behavior.ReachAIDestination();
                }
            }
        }

    }

    public void ReactoPlayer(PlayerMovement player)
    {
        _behavior.ReactToPlayer(player);
    }



    public void SetDestination(Vector3 destination)
    {
        _agent.destination=destination;
    }

    internal void fleeToPoint()
    {
        AIRunAway ra = (AIRunAway)_behavior;

        if (ra!=null)
        {
            ra.fleeToPoint();
        }
    }
}
