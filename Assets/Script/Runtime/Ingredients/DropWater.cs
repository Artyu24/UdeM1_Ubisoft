using UnityEngine;

public class DropWater : MonoBehaviour
{
    public void DropWaterBelow()
    {
        RaycastHit[] rayHits = Physics.BoxCastAll(transform.position, new Vector3(0.5f, 0.5f, 0.5f), -transform.up, Quaternion.identity, 100f);
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
