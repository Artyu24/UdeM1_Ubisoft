using DG.Tweening;
using UnityEngine;
public delegate void OnWaterDry(WaterPuddle wp);
public class WaterPuddle : MonoBehaviour
{
    [SerializeField] private bool _doesContainsOil;

    [field: SerializeField] private int CleanValue = 3;
    public OnWaterDry onWaterDry;


    [SerializeField] private float initialScale = 1f;
    private int maxCleanValue;
    
    private void Awake()
    {
        maxCleanValue = CleanValue; 
        transform.localScale = Vector3.one * initialScale;
        
    }
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
        UpdateScale();
        if (CleanValue == 0) 
        {
            DryWater();
        }
        return CleanValue;
    }
    public void DryWater()
    {
        //transform.DOScale(Vector3.zero, 1f);
        onWaterDry.Invoke(this);
        onWaterDry=null;
    }
    private void UpdateScale()
    {
        // Calcul du facteur proportionnel
        float scaleFactor = (float)CleanValue / maxCleanValue;
        // Calcul de la nouvelle échelle
        Vector3 newScale = Vector3.one * initialScale * scaleFactor;
        // Animation de l'échelle avec DOTween
        transform.DOScale(newScale, 0.5f);
    }

}
