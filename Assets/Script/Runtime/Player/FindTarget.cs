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
}
