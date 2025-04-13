using UnityEngine;
using System.Collections;
using NaughtyAttributes;
public class AIRunAway : AIBehavior
{
    [Header("Running away")]
    [SerializeField] private Transform _reactionZone;
    [SerializeField] private Transform _leavePoint;
    [SerializeField] private string _objectType;
    public bool IsRunningAway { get; set; }

    void Start()
    {
        if (AiBrain.WanderPoints.Count==0)
            return;

        //AiBrain.SetDestination(_destinationPoint.position);
        Wander();
        AIEventHandler.instance.Ai.Add(this); 
    }
    public void LookForObject(string objectType)
    {
        AIObject aIObject = AiBrain.LineOfSight.GetSightObjectByType(objectType);
        AiBrain.State = npcState.LookingArround;
        if (aIObject == null)
        {
            AiBrain.State = npcState.leaving;
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
            IsRunningAway = false;
        }
    }
    protected IEnumerator PushPlayer(PlayerMovement playerMovement)
    {
        AiBrain._animator.SetTrigger("Push");
        playerMovement.OnIAPush(transform.position);
        yield return new WaitForSeconds(1f);
        _reactionCoroutine = null;
    }
    [Button]
    public void fleeToPoint()
    {
        AiBrain._agent.destination = _reactionZone.position;
        AiBrain.State = npcState.leaving;
        IsRunningAway = true;
        AiBrain.Onflee.Invoke();
        StopAllCoroutines();// kill all action running
        AiBrain.State=npcState.Reacting;
    }

    public void LeaveZone()
    {
        AiBrain.SetDestination(_leavePoint.position);
    }
    public override void ReactToPlayer(PlayerMovement player)
    {
        if (AiBrain.State == npcState.chasing || _isGnoringPlayer || AiBrain.State == npcState.Stunned) return;
        if (player == null) return;
        if (_reactionCoroutine != null) return;//a move

        _reactionCoroutine = StartCoroutine(PushPlayer(player));
    }
    public override void ReachAIDestination()
    {
        base.ReachAIDestination();

        switch(AiBrain.State)
        {
            case npcState.Reacting:
                AiBrain.OnReachDestination.Invoke();
                LookForObject(_objectType);
                break;

        }
    }
}
