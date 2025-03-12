using UnityEngine;
using UnityEngine.Events;
public class AIEventDispatcher : MonoBehaviour
{
    public UnityEvent<Transform> RunToReactionZon;
    

    public void AARunAway()
    {
        AIEventHandler.instance.MakeAIRunAway(transform);
    }
}
