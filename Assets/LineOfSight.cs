using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] private LayerMask viewMask;
    [SerializeField] private float _reactionTime = 1f;
    [SerializeField] private float _playerDistanceBeforePush = 1f;
    [field: SerializeField] public List<AIObject> InSight { get; set; }
    
    private Coroutine _pushPlayer;

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
        if(!SightCheck(other)) return;
        AddToSighList(other);
        CheckIsPlayer(other.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!SightCheck(other)) return;
        AddToSighList(other);
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
    private bool SightCheck(Collider other)
    {
        RaycastHit hit;
        Physics.Raycast(transform.parent.position, (other.transform.position - transform.parent.position).normalized, out hit, 100, viewMask);
        Debug.DrawRay(transform.parent.position, (other.transform.position - transform.parent.position).normalized * hit.distance, Color.yellow);
        return hit.collider;
    }
    private void CheckIsPlayer(GameObject go)//method a refacto pour plus solide
    {
        if (go == null) return;
        Debug.Log((Vector3.Distance(go.transform.position, transform.parent.position)));
        if (Vector3.Distance(go.transform.position, transform.parent.position) > _playerDistanceBeforePush) return;
        if(go.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            //playerMovement.OnIAPush(transform.parent.position);
            if (_pushPlayer != null) return;
            _pushPlayer=StartCoroutine(PushPlayer(playerMovement));
        }
    }
    private IEnumerator PushPlayer(PlayerMovement playerMovement)
    {
        playerMovement.OnIAPush(transform.parent.position);
        yield return new WaitForSeconds(1f);
        _pushPlayer=null;

    }

}
