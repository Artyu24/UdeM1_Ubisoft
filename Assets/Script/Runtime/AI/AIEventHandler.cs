using System.Collections.Generic;
using UnityEngine;

public class AIEventHandler : MonoBehaviour
{
    [field:SerializeField]public List<AIRunAway> Ai {  get; set; }
    public static AIEventHandler instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        Ai = new List<AIRunAway>();
    }
    public void MakeAIRunAway(Transform position)
    {
        foreach (var ai in Ai) 
        {
            float distance = Vector2.Distance(position.position,ai.transform.position);
            if (distance < 5) 
            {
                ai.fleeToPoint();
            }
            Debug.Log(distance);   
        }
    }
}
