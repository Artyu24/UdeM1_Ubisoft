using UnityEngine;

public class TrophyBehaviour : MonoBehaviour
{
    [SerializeField]int ObjectCounter;

    public void AddObject()
    {
        // new object
        ObjectCounter++;
    }
}
