using System.Collections;
using NaughtyAttributes;
using UnityEngine;

public class DropWater : MonoBehaviour
{
    [SerializeField, Layer] private int _groundLayer;
    
    [SerializeField] private WaterPuddle _waterPuddlePrefab;

    [SerializeField] private ParticleSystem _waterParticle;

    private bool _dropPuddle;
    
    public void DropWaterBelow(bool mustDropPuddle)
    {
        if(_waterParticle != null)
            _waterParticle.Play();

        _dropPuddle = mustDropPuddle;
        
        StartCoroutine(LaunchBehaviourDelay());
    }

    private void DoBehaviour()
    {
        RaycastHit[] rayHits = Physics.BoxCastAll(transform.position, new Vector3(0.5f, 0.5f, 0.5f), -transform.up, Quaternion.identity, 100f);
        if(rayHits.Length == 0)
            return;
        
        for (int i = 0; i < rayHits.Length; i++)
        {
            //Fill Bucket
            Bucket bucket = rayHits[i].transform.GetComponent<Bucket>();
            if (bucket != null)
            {
                bucket.FillBucket();
                break;
            }
            
            if(!_dropPuddle)
                continue;

            //Already a Puddle
            WaterPuddle puddle = rayHits[i].transform.GetComponent<WaterPuddle>();
            if (puddle != null)
                break;
                
            //Place Puddle on Ground
            if (rayHits[i].transform.gameObject.layer == _groundLayer)
            {
                WaterPuddle waterPuddle = Instantiate(_waterPuddlePrefab, rayHits[i].point, Quaternion.identity);
                waterPuddle.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
                break;
            }
        }
    }
    
    private IEnumerator LaunchBehaviourDelay()
    {
        yield return new WaitForSeconds(1f);
        DoBehaviour();
    }
}
