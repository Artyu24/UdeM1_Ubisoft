using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
public delegate void OnWaterDry(WaterPuddle wp);
public class WaterPuddle : MonoBehaviour
{
    [SerializeField] private bool _doesContainsOil;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField, Required] Transform _Watermesh;
    [SerializeField] private float initialScale = 1f;
    [SerializeField] private int CleanValue = 3;

    public OnWaterDry onWaterDry;
    private int maxCleanValue;
    public GameObject Sign { get;set; }

    public UnityEvent OnDry;
    private void Awake()
    {
        if(AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.SFX_WATERPUDDLE_APPEAR);
        
        maxCleanValue = CleanValue; 
        _Watermesh.transform.localScale = Vector3.one * initialScale;
        
    }
    public void TriggerEnter(Collider other)
    {
        ISlideable slideableCharacter = other.transform.GetComponent<ISlideable>();
        if (slideableCharacter != null)
        {
            slideableCharacter.OnSlide(_doesContainsOil);
            return;
        }
        
        Oil oil = other.transform.GetComponent<Oil>();
        if (oil != null)
        {
            oil.MeltsOil(() =>
            {
                _doesContainsOil = true;
                _meshRenderer.material.color = Color.blue;
                
                if(AudioManager.instance != null)
                    AudioManager.instance.PlayRandom(SoundState.SFX_MIXWATER_AND_OIL);
            });
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
            Destroy(gameObject);
        }
        OnDry.Invoke();
    }
    private void UpdateScale()
    {
        // Calcul du facteur proportionnel
        float scaleFactor = (float)CleanValue / maxCleanValue;
        // Calcul de la nouvelle �chelle
        Vector3 newScale = Vector3.one * initialScale * scaleFactor;
        // Animation de l'�chelle avec DOTween
        _Watermesh.transform.DOScale(newScale, 0.5f);
    }

}
