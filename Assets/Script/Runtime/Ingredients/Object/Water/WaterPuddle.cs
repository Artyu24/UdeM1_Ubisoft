using DG.Tweening;
using UnityEngine;

public class WaterPuddle : MonoBehaviour
{
    [SerializeField] private bool _doesContainsOil;

    [field: SerializeField] private int CleanValue = 3;
    
    private void OnTriggerEnter(Collider other)
    {
        ISlideable slideableCharacter = other.transform.GetComponent<ISlideable>();
        if (slideableCharacter != null)
        {
            slideableCharacter.OnSlide(_doesContainsOil);
        }
    }
    public int CleanPuddle()
    {
        CleanValue--;
        if (CleanValue == 0) 
        { 
            transform.DOScale(Vector3.zero, 1f);
        }
        return CleanValue;
    }
}
