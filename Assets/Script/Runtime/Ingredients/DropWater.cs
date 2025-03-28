using UnityEngine;

public class DropWater : MonoBehaviour
{
    public void DropWaterBelow()
    {
        RaycastHit[] rayHits = Physics.RaycastAll(transform.position, -transform.up);
        if(rayHits.Length == 0)
            return;

        for (int i = 0; i < rayHits.Length; i++)
        {
            Bucket bucket = rayHits[i].transform.GetComponent<Bucket>();
            if (bucket != null)
            {
                bucket.FillBucket();
                break;
            }
        }
    }
}
