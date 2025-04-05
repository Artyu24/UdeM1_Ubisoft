using System;
using NaughtyAttributes;
using UnityEngine;

public class Throne : MonoBehaviour
{
    public static event Action<int> OnThroneUpdated;
    [SerializeField]int ObjectCounter;

    [Button]
    public void AddObject()
    {
        // new object
        ObjectCounter++;
        Debug.Log("Throne Count : " + ObjectCounter);
        OnThroneUpdated?.Invoke(ObjectCounter); // broadcast event
    }
}
