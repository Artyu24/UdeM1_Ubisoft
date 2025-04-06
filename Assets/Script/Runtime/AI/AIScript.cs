using DG.Tweening;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEditor.SceneManagement;
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
    yeetPlayer,
    Slide
}
public enum NPCType
{
    notmoving,
    RunAway,
    cleaner,
    Guarde,
    Wanderer,
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
    [Header("Player Reactions")]
    [field: SerializeField, OnValueChanged("TypeChangeCallback")] NPCType TypeNPC { get; set; }

    [SerializeField, Required("Must not be null "), Tooltip("add the corresponding componement (eg AIChase...) and put it there")]
    private AIBehavior _behavior;

    [Header("Wander")]
    [field: SerializeField] public List<Transform> WanderPoints { get; set; }
    [field: SerializeField] public float WanderCoolDownTime { get; set; } = 1f;


    [Header("AI INFO")]
    [SerializeField] private LayerMask _viewMak;
    [field:SerializeField, Required] public NavMeshAgent _agent {  get; set; }
    public LineOfSight LineOfSight { get; set; }//auto ref
    [SerializeField] float _stopingdistance=0;

    public npcState State { get; set; }


    private void TypeChangeCallback()
    {
        DestroyImmediate(GetComponent<AIBehavior>());
        switch (TypeNPC)
        {
            case NPCType.notmoving:
                break;
            case NPCType.RunAway:
                _behavior=gameObject.AddComponent<AIRunAway>();
                break;
            case NPCType.Guarde:
                _behavior=gameObject.AddComponent<AiPlayerChase>();
                break;
            case NPCType.Wanderer:
                _behavior= gameObject.AddComponent<AIBehavior>();
                break;
            case NPCType.cleaner:
                _behavior = gameObject.AddComponent<AICleaner>();
                break;
            default:
                break;
        }
        _behavior.AiBrain=this;
    }
    Vector3 _lastPlayerSeenPosition = Vector3.zero;
    private Coroutine _reactionCoroutine;
    private Coroutine _lookingArroundCorou;


    [Header("grab")]
    [SerializeField] Transform _grabPosition;
    [field: SerializeField] public float StunTime { get; set; } = 3f;
    [SerializeField] Transform _DropPosition;
    private PlayerMovement _grabbedPlayer;

    [Foldout("Event")]
    public UnityEvent Onflee;
    [Foldout("Event")]
    public UnityEvent OnReachDestination;
    [Foldout("Event")]
    public UnityEvent OnReachReactionDestination;
    [Foldout("Event")]
    public UnityEvent OnHited;



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
            if (_agent.remainingDistance <= _agent.stoppingDistance+_stopingdistance)
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
        _agent.stoppingDistance = +_stopingdistance;
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
