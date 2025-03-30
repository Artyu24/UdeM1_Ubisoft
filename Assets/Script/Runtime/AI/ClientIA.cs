using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClientIA : MonoBehaviour, ISlideable
{
    [Header("IA")]
    [SerializeField] private NavMeshAgent _clientAgent;
    [SerializeField] private List<Transform> _wanderPoints = new List<Transform>();
    private int _wanderIndex = 0;

    [Header("Water Slide")] 
    [SerializeField] private float _fallenTime = 2;
    
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
    
    public void OnRangeTriggerEnter(Collider other)
    {
        PlayerMovement pM = other.gameObject.GetComponent<PlayerMovement>();
        if (pM != null)
        {
            pM.OnIAPush(transform.position);
        }
    }

    public void OnSlide(bool doesContainsOil)
    {
        if (!doesContainsOil)
        {
            //Fall
            _clientAgent.isStopped = true;

            StartCoroutine(IAFalling());
        }
        else
        {
            //Slide
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit))
            {
                _clientAgent.destination = hit.point;
                _wanderIndex--;
            }
            return;
        }
    }

    private IEnumerator IAFalling()
    {
        yield return new WaitForSeconds(_fallenTime);
        _clientAgent.isStopped = false;
    }
    
}
