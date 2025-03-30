using UnityEngine;

public class WaterPuddle : MonoBehaviour
{
    [SerializeField] private bool _doesContainsOil;
    
    private void OnTriggerEnter(Collider other)
    {
        ISlideable slideableCharacter = other.transform.GetComponent<ISlideable>();
        if (slideableCharacter != null)
        {
            slideableCharacter.OnSlide(_doesContainsOil);
        }
    }
}
