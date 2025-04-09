using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
public delegate void OnWaterDry(WaterPuddle wp);
public class WaterPuddle : MonoBehaviour
{
    [SerializeField] private bool _doesContainsOil;
    [SerializeField, Required] Transform _Watermesh;
    [SerializeField] private float initialScale = 1f;
    [SerializeField] private int CleanValue = 3;

    public OnWaterDry onWaterDry;
    private int maxCleanValue;
    public GameObject Sign { get;set; }

    public UnityEvent OnDry;
    private void Awake()
    {
        maxCleanValue = CleanValue; 
        _Watermesh.transform.localScale = Vector3.one * initialScale;
        
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
        if(Sign.TryGetComponent<SlideSign>(out SlideSign sign))
        {
            sign.disappear();
        }
        OnDry.Invoke();
    }
    private void UpdateScale()
    {
        // Calcul du facteur proportionnel
        float scaleFactor = (float)CleanValue / maxCleanValue;
        // Calcul de la nouvelle échelle
        Vector3 newScale = Vector3.one * initialScale * scaleFactor;
        // Animation de l'échelle avec DOTween
        _Watermesh.transform.DOScale(newScale, 0.5f);
    }

}
