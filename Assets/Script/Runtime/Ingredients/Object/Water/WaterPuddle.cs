using UnityEngine;

public class WaterPuddle : MonoBehaviour
{
    [SerializeField] private bool _doesContainsOil;
    [SerializeField] private MeshRenderer _meshRenderer;
    
    private void OnTriggerEnter(Collider other)
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
            Destroy(oil.gameObject);
            _doesContainsOil = true;
            _meshRenderer.material.color = Color.blue;
        }
    }
}
