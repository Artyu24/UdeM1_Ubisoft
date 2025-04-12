using System;
using DG.Tweening;
using UnityEngine;

public class SnapBucket : MonoBehaviour
{
    [SerializeField] private Transform _snapPos;

    private void OnTriggerEnter(Collider other)
    {
        Bucket bucket = other.transform.GetComponent<Bucket>();
        if (bucket != null)
        {
            if (!bucket.IsGrabbed)
            {
                bucket.IsInMovement = true;
                bucket.transform.DOMove(_snapPos.position, 0.5f).OnComplete(() => { bucket.IsInMovement = false;});
            }
        }
    }
}
