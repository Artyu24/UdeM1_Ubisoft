using System;
using UnityEngine;

public class SaveZone : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerData>())
        {
            Vector3 pos = other.transform.position;
            other.transform.position = new Vector3(pos.x, 2f, pos.z);
        }
    }
}
