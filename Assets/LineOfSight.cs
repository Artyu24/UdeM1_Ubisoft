using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private LayerMask viewMask;
    [SerializeField] private float _reactionTime = 1f;
    [field: SerializeField] public List<AIObject> InSight { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        RaycastHit hit;
        Physics.Raycast(transform.parent.position, (other.transform.position - transform.parent.position).normalized, out hit, 100, viewMask);
        Debug.DrawRay(transform.parent.position, (other.transform.position - transform.parent.position).normalized * hit.distance, Color.yellow);
        AddToSighList(hit.collider);
    }
    private void OnTriggerEnter(Collider other)
    {
        RaycastHit hit;
        Physics.Raycast(transform.parent.position, (other.transform.position - transform.parent.position).normalized,out hit, 100, viewMask);
        Debug.DrawRay(transform.parent.position, (other.transform.position - transform.parent.position).normalized * hit.distance, Color.yellow);
        AddToSighList(hit.collider);
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent<AIObject>(out AIObject objectAI))
            if (InSight.Contains(objectAI))
                InSight.Remove(objectAI);
    }

    private IEnumerator processSight()
    {
        yield return new WaitForSeconds(_reactionTime);
    }
    public void AddToSighList(Collider other)
    {
        if (other != null && other.gameObject.TryGetComponent<AIObject>(out AIObject objectAI))
        {
            if (!InSight.Contains(objectAI))
                InSight.Add(objectAI);
        }
    }
    public AIObject GetSightObjectByType(string type)
    {
        foreach(AIObject ob in InSight)
        {
            if(ob.ObjectType==type)
                return ob;
        }
        return null;
    }
}
