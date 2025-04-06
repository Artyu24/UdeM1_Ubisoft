using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private LayerMask viewMask;
    [SerializeField] private float _reactionTime = 1f;
    [SerializeField] private float _rangePlayerView = 1f;
    [field: SerializeField] public List<AIObject> InSight { get; set; }
    [field: SerializeField] public List<PlayerMovement> PlayerInSight { get; set; }
    [field: SerializeField] public List<PlayerMovement> Inrange { get; set; }
    private Coroutine _pushPlayer;
    [SerializeField] private AIScript _AiBrain;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _AiBrain.LineOfSight=this;
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.parent.position, transform.parent.forward * _rangePlayerView,Color.red);
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        if (SightCheck(other))
        {
            AddToSighList(other);
            
        }
        else
        {
            RemoveToSight(other.gameObject);
            return;
        }
        CheckIsPlayer(other.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!SightCheck(other)) return;
        AddToSighList(other);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<AIObject>(out AIObject objectAI))
            if (InSight.Contains(objectAI))
            {
                InSight.Remove(objectAI);
                return;
            }
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            if (PlayerInSight.Contains(playerMovement))
            {
                PlayerInSight.Remove(playerMovement);
                return;
            }
        }

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
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            if(!PlayerInSight.Contains(playerMovement))
                PlayerInSight.Add(playerMovement);
        }
    }
    public void RemoveToSight(GameObject other)
    {
        if (other != null && other.gameObject.TryGetComponent<AIObject>(out AIObject objectAI))
        {
            if (InSight.Contains(objectAI))
                InSight.Remove(objectAI);
        }
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            if (PlayerInSight.Contains(playerMovement))
                PlayerInSight.Remove(playerMovement);
        }
    }
    public void AddToSighList(GameObject other)
    {
        if (other != null && other.gameObject.TryGetComponent<AIObject>(out AIObject objectAI))
        {
            if (!InSight.Contains(objectAI))
                InSight.Add(objectAI);
        }
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            if (!PlayerInSight.Contains(playerMovement))
                PlayerInSight.Add(playerMovement);
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
    private Collider SightCheck(Collider other)
    {
        RaycastHit hit;
        Physics.Raycast(transform.parent.position, (other.transform.position - transform.parent.position).normalized, out hit, 100, viewMask);
        Debug.DrawRay(transform.parent.position, (other.transform.position - transform.parent.position).normalized * hit.distance, Color.yellow);
        if(hit.collider != other)
            return null;
        else
            return hit.collider;
    }
    private Collider SightCheck(GameObject other)
    {
        RaycastHit hit;
        Physics.Raycast(transform.parent.position, (other.transform.position - transform.parent.position).normalized, out hit, 100, viewMask);
        Debug.DrawRay(transform.parent.position, (other.transform.position - transform.parent.position).normalized * hit.distance, Color.yellow);
        if (hit.collider.gameObject != other)
            return null;
        else
            return hit.collider;
    }
    private void CheckIsPlayer(GameObject go)//method a refacto pour plus solide
    {
        if (go == null) return;
        if (!go.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement)) return;


        //Debug.Log((Vector3.Distance(go.transform.position, transform.parent.position)));
        if (Vector3.Distance(go.transform.position, transform.parent.position) > (_AiBrain.State == npcState.chasing? _rangePlayerView :  _rangePlayerView)) return;
            _AiBrain.ReactoPlayer(playerMovement);
    }

    

}
