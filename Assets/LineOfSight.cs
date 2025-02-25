using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private LayerMask viewMask;
    [SerializeField] private float _reactionTime = 1f;
    [field: SerializeField] public List<GameObject> InSight { get; set; } 
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
        Physics.Raycast(transform.position, (other.transform.position - transform.position).normalized, out hit, 100, viewMask);
        Debug.DrawRay(transform.position, (other.transform.position - transform.position).normalized * hit.distance, Color.yellow);
        if (hit.collider != null)
            if(!InSight.Contains(hit.collider.gameObject))
                InSight.Add(other.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        RaycastHit hit;
        Physics.Raycast( transform.position,(other.transform.position - transform.position).normalized,out hit, 100, viewMask);
        Debug.DrawRay(transform.position, (other.transform.position - transform.position).normalized * hit.distance, Color.yellow);
        if( hit.collider != null )
            if (!InSight.Contains(hit.collider.gameObject))
                InSight.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if(InSight.Contains(other.gameObject))
            InSight.Remove(other.gameObject);
    }

    private IEnumerator processSight()
    {
        yield return new WaitForSeconds(_reactionTime);
    }
}
