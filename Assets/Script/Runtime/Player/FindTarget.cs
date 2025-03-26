using System;
using UnityEngine;
using UnityEngine.AI;

public class FindTarget : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _moveToTarget;

    private void Awake()
    {
        Destroy(gameObject, 10f);
    }

    public void Init(Vector3 pos)
    {
        _moveToTarget.destination = pos;
    }

    private void Update()
    {
        if (!_moveToTarget.pathPending)
        {
            if (_moveToTarget.remainingDistance <= _moveToTarget.stoppingDistance)
            {
                if (!_moveToTarget.hasPath || _moveToTarget.velocity.sqrMagnitude == 0f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
