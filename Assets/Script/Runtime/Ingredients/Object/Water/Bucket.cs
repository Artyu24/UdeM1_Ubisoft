using UnityEngine;

public class Bucket : GrabObject
{
    [SerializeField] private GameObject _waterLevel;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private WaterPuddle _waterPuddlePrefab;

    private bool _isInMovement;
    public bool IsInMovement { set => _isInMovement = value; }
    
    private bool _isWaterInside;

    public void FillBucket()
    {
        if(_isWaterInside)
            return;
        
        _waterLevel.SetActive(true);
        _isWaterInside = true;
    }

    public override bool OnGrab(Transform catcher)
    {
        if (_isInMovement)
            return false;
        
        return base.OnGrab(catcher);
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
        RaycastHit[] hits = Physics.RaycastAll(transform.position, -Vector3.up, 100f, _layerMask); 
        if (hits.Length != 0)
        {
            foreach (RaycastHit hit in hits)
            {
                WaterPuddle waterPuddle = Instantiate(_waterPuddlePrefab, hit.point, Quaternion.identity);
                waterPuddle.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
            }
        }
    }
}
