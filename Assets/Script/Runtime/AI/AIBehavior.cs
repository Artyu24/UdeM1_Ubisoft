using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using UnityEngine;

public class AIBehavior : MonoBehaviour, IInteractable
{
    [field: SerializeField] public AIScript AiBrain { get; set; }
    protected bool _isGnoringPlayer = false;
    protected Coroutine _lookingArroundCorou;
    protected Coroutine _reactionCoroutine;
    protected Coroutine _wandererDelay=null;

    protected Transform _destinationPoint;

    int _WanderIndex = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_destinationPoint = AiBrain.WanderPoints[Random.Range(0, AiBrain.WanderPoints.Count - 1)];
        if (AiBrain.WanderPoints.Count==0)
            return;

        //AiBrain.SetDestination(_destinationPoint.position);
        Wander();
    }
    protected void InterruptCorouAction(Coroutine corouAction)
    {
        if (corouAction == null) return;
        StopCoroutine(corouAction);
        corouAction = null;
    }
    protected IEnumerator IgnorePlayer()
    {
        _isGnoringPlayer = true;
        yield return new WaitForSeconds(1);
    }
    protected IEnumerator LookArround()
    {

        AiBrain.State = npcState.LookingArround;
        transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y + 90, transform.rotation.z), 0.5f);
        yield return new WaitForSeconds(1f);
        transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y - 90, transform.rotation.z), 0.5f);
        yield return new WaitForSeconds(1f); ;


        AiBrain.State = npcState.wandering;
        _lookingArroundCorou = null;

        Wander();
    }
    public virtual void ReactToPlayer(PlayerMovement player)
    {
        if (AiBrain.State == npcState.chasing || _isGnoringPlayer) return;
        if (player == null) return;
        if (_reactionCoroutine != null) return;
    }
    public virtual void ReachAIDestination()
    {
        switch (AiBrain.State)
        {
            case npcState.wandering:
                if (_wandererDelay == null)
                {
                    _wandererDelay = StartCoroutine(DelayBeforeWandering());
                }
                break;
            case npcState.chasing:
                if (_lookingArroundCorou == null)
                    _lookingArroundCorou = StartCoroutine(LookArround());
                break;
            case npcState.LookingArround:
                break;
            default:
                break;
        }
    }
    public void Wander()
    {
        if (_WanderIndex >= AiBrain.WanderPoints.Count - 1)
            _WanderIndex = 0;
        else
            _WanderIndex++;

        _destinationPoint = AiBrain.WanderPoints[_WanderIndex];
        AiBrain.SetDestination(_destinationPoint.position);
    }
    protected IEnumerator DelayBeforeWandering()
    {
        yield return new WaitForSeconds(AiBrain.WanderCoolDownTime);
        Wander();
        _wandererDelay = null;
    }
    
    public virtual void Interact()
    {

    }
    protected IEnumerator StunnedDelay()
    {
        AiBrain.State = npcState.Stunned;
        _isGnoringPlayer = true;
        AiBrain._agent.isStopped = true;
        yield return new WaitForSeconds(AiBrain.StunTime);
        _isGnoringPlayer = false;
        AiBrain._agent.isStopped = false;
        AiBrain.State = npcState.wandering;
        Wander();
    }


}
