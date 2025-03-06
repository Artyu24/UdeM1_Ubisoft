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
    [SerializeField,Required] private LineOfSight _lineOfSight;
    [SerializeField] private LayerMask _viewMak;
    [SerializeField, Required] private NavMeshAgent _agent;
    [SerializeField] private NavMeshSurface _navMesh;
    [SerializeField] Transform _target;


    
    [field:SerializeField]public List<GameObject> interest { get; set; }

    public UnityEvent Onflee;
    public UnityEvent OnReachDestination;
    public UnityEvent OnReachReactionDestination;
    void Start()
    {
        Wander();
    }
    void Update()
    {
        if (!_agent.pathPending)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                {
                    if (!_isRunningAway && _wandererDelay==null)
                    {
                        OnReachDestination.Invoke();
                        //transform.rotation = Quaternion.Euler(_guardRotation);
                        transform.DORotate(_guardRotation,0.5f);
                        _wandererDelay = StartCoroutine(DelayBeforeWandering());
                    }
                    else if (_isRunningAway)
                    {
                        OnReachReactionDestination.Invoke();
                        LookForObject(_objectType);
                        
                    }
                    //use the closest item at position? like a pick or something?
                    //stay for an amount of time before going back to previous pos?
                }
            }
        }
    }
    [Button]
    public void fleeToPoint()
    {
        //Vector3 dir = (target.position - transform.position).normalized;
        //agent.SetDestination(transform.position - (dir * 2));
        _agent.destination = _reactionZone.position;
        _isRunningAway=true;
        StopAllCoroutines();
        //StopCoroutine(_wandererDelay);
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
    public void RunTo(Transform PosToGo)
    {
        _agent.destination = _target.position;
        
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
        //void Update()
        //{
        //    RaycastHit hit;
        //    Physics.Raycast( transform.position,(interest[0].transform.position - transform.position).normalized,out hit, 100, viewMak);
        //    Debug.DrawRay(transform.position, (interest[0].transform.position - transform.position).normalized * hit.distance, Color.yellow);

        //    if (hit.collider == interest[0].GetComponent<Collider>())//temp
        //    {
        //        if(Vector3.Distance(transform.position, interest[0].transform.position)<5&& agent.destination != interest[0].transform.position )
        //        {
        //            agent.destination= interest[0].transform.position;
        //        }
        //    }
        //}

        /* un system de vision des element d'interet,
         tire un raycast si les points d'interet les plus proches, liste dynamique d'objet le points le plus proche a plus d'interet

         */
         
}
