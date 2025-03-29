using UnityEngine;

public class Bucket : GrabObject
{
    [SerializeField] private GameObject _waterLevel;

    private bool _isWaterInside;
    
    public void FillBucket()
    {
        if(_isWaterInside)
            return;
        
        _waterLevel.SetActive(true);
        _isWaterInside = true;
    }

    private void EmptyBucket()
    {
        if(!_isWaterInside)
            return;
        
        _waterLevel.SetActive(false);
        _isWaterInside = false;
        
        //Fell off water on ground
    }
}
