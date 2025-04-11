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
    [SerializeField] bool _willTurnIntoGuarde=false;

    [SerializeField, Required("Must not be null "), Tooltip("add the corresponding componement (eg AIChase...) and put it there")]
    private AIBehavior _behavior;

    [Header("Wander")]
    [field: SerializeField] public List<Transform> WanderPoints { get; set; }
    [field: SerializeField] public float WanderCoolDownTime { get; set; } = 1f;


    [Header("AI INFO")]
    [SerializeField] private LayerMask _viewMak;
    [field:SerializeField, Required] public NavMeshAgent _agent {  get; set; }
    public LineOfSight LineOfSight { get; set; }//auto ref

    public npcState State { get; set; }

    [SerializeField,Tooltip("if the npc have an object")]private GrabObject _grabObject;

    //[SerializeField, Tooltip("position where the object willsnap")]private Transform _objectpos;

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
    [field: SerializeField,Tooltip("position where the player is moved when transported")] public Transform GrabPosition { get; set; }
    [field: SerializeField] public float StunTime { get; set; } = 3f;
    [SerializeField, Tooltip("Position where the player is going to be dropped")] Transform _DropPosition;
    public Transform DropPosition { get=>_DropPosition; set => _DropPosition = value; }
    
    private PlayerMovement _grabbedPlayer;


    [Foldout("Event")]
    public UnityEvent Onflee;
    [Foldout("Event")]
    public UnityEvent OnReachDestination;
    [Foldout("Event")]
    public UnityEvent OnHited;
    [Foldout("Event")]
    public UnityEvent OnFall;



    private void OnDrawGizmosSelected()
    {

    }
    void Start()
    {
        if (_grabObject != null)
        {
            _grabObject.OnGrab(transform);
        }

        if(_willTurnIntoGuarde)
            PlayerManager.instance.OnGrabFinalObject.AddListener(TurnIntoGuard);
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

    public void fleeToPoint()
    {
        AIRunAway ra = (AIRunAway)_behavior;

        if (ra!=null)
        {
            ra.fleeToPoint();
        }
    }
    [Button]
    private void TurnIntoGuard()
    {
        _behavior = gameObject.AddComponent<AiPlayerChase>();
        _behavior.AiBrain=this;
        PlayerManager.instance.OnGrabFinalObject.RemoveListener(TurnIntoGuard);
        
    }
    [Button]
    public void DropItem()
    {
        if(_grabObject == null) return;
        _grabObject.OnRelease();
    }

    public void PlayAudio(SoundState audioState)
    {
        AudioManager.instance.PlayRandom(audioState);
    }
}
