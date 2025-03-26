using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClientIA : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _clientAgent;
    [SerializeField] private List<Transform> _wanderPoints = new List<Transform>();
    private int _wanderIndex = 0;
    
    private void Awake()
    {
        if(_wanderPoints.Count != 0)   
            _clientAgent.destination = _wanderPoints[0].position;
    }
    
    private void Update()
    {
        if(_wanderPoints.Count == 0)
            return;
        
        if (!_clientAgent.pathPending)
        {
            if (_clientAgent.remainingDistance <= _clientAgent.stoppingDistance)
            {
                if (!_clientAgent.hasPath || _clientAgent.velocity.sqrMagnitude == 0f)
                {
                    _wanderIndex++;
                    if (_wanderPoints.Count <= _wanderIndex)
                        _wanderIndex = 0;

                    _clientAgent.destination = _wanderPoints[_wanderIndex].position;
                }
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement pM = other.gameObject.GetComponent<PlayerMovement>();
        if (pM != null)
        {
            pM.OnIAPush(transform.position);
        }
    }
}
