using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIEventHandler : MonoBehaviour
{
    [field:SerializeField]public List<AIScript> Ai {  get; set; }
    public static AIEventHandler instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        Ai = new List<AIScript>();
    }
    public void MakeAIRunAway(Transform position)
    {
        foreach (var ai in Ai) 
        {
            //on check la distance de chaque IA
            //
            float distance = Vector2.Distance(position.position,ai.transform.position);
            if (distance < 5) 
            {
                ai.fleeToPoint();
            }
            Debug.Log(distance);   
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        //MakeAIRunAway(transform);
    }
}
