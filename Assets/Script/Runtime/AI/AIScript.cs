using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class AIScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private NavMeshSurface navMesh;
    [SerializeField] Transform target;
    void Start()
    {
        agent.destination = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
