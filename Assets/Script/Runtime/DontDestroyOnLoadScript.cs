using System;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
