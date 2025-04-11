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
        if(AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.SFX_GOUTELETTE_EAU_VALVE);
        
        RaycastHit[] rayHits = Physics.BoxCastAll(transform.position, new Vector3(0.5f, 0.5f, 0.5f), -transform.up, Quaternion.identity, 100f);
        if(rayHits.Length == 0)
            return;
        
        //Memory
        DropWaterAction actionState = DropWaterAction.NONE;
        Bucket bucket = null;
        Vector3 puddlePos = default;
        
        //Select Action
        for (int i = 0; i < rayHits.Length; i++)
        {
            //Fill Bucket
            bucket = rayHits[i].transform.GetComponent<Bucket>();
            if (bucket != null)
            {
                actionState = DropWaterAction.FILL_BUCKET;
                break;
            }
            
            if(!_dropPuddle)
                continue;

            //Already a Puddle
            WaterPuddle puddle = rayHits[i].transform.GetComponent<WaterPuddle>();
            if (puddle != null)
            {
                if (actionState != DropWaterAction.FILL_BUCKET)
                    actionState = DropWaterAction.ALREADY_PUDDLE;
            }
                
            //Place Puddle on Ground
            if (rayHits[i].transform.gameObject.layer == _groundLayer && actionState != DropWaterAction.SPAWN_PUDDLE)
            {
                if (actionState == DropWaterAction.NONE)
                    actionState = DropWaterAction.SPAWN_PUDDLE;

                puddlePos = rayHits[i].point;
            }
        }

        //Do Action
        switch (actionState)
        {
            case DropWaterAction.NONE:
            case DropWaterAction.ALREADY_PUDDLE:
                break;
            case DropWaterAction.FILL_BUCKET:
                if(bucket != null)
                    bucket.FillBucket();
                break;
            case DropWaterAction.SPAWN_PUDDLE:
                WaterPuddle waterPuddle = Instantiate(_waterPuddlePrefab, puddlePos, Quaternion.identity);
                waterPuddle.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
                break;
        }
    }
    
    private IEnumerator LaunchBehaviourDelay()
    {
        yield return new WaitForSeconds(1f);
        DoBehaviour();
    }
    
    private enum DropWaterAction
    {
        NONE,
        FILL_BUCKET,
        ALREADY_PUDDLE,
        SPAWN_PUDDLE
    }
}
