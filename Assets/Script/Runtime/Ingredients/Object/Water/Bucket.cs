using UnityEngine;

public class Bucket : GrabObject
{
    [SerializeField] private GameObject _waterLevel;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private WaterPuddle _waterPuddlePrefab;

    private bool _isWaterInside;
    
    public void FillBucket()
    {
        if(_isWaterInside)
            return;
        
        _waterLevel.SetActive(true);
        _isWaterInside = true;
    }

    public override void OnRelease()
    {
        base.OnRelease();
        
        EmptyBucket();
    }

    private void EmptyBucket()
    {
        if(!_isWaterInside)
            return;
        
        _waterLevel.SetActive(false);
        _isWaterInside = false;
        
        //Fell off water on ground
        RaycastHit hit; 
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, _layerMask))
        {
            WaterPuddle waterPuddle = Instantiate(_waterPuddlePrefab, hit.point, Quaternion.identity);
            waterPuddle.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
        }
    }
}
