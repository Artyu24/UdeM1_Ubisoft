using UnityEngine;

[SelectionBase]
public class Breakable : MonoBehaviour
{
    [SerializeField] GameObject intact;
    [SerializeField] GameObject broken;

    BoxCollider bc;

    private void Awake()
    {
        intact.SetActive(true);
        broken.SetActive(false);

        bc = GetComponent<BoxCollider>();
    }

    private void OnMouseDown()
    {
        Break();
    }

    private void Break()
    {
        intact.SetActive(false);
        broken.SetActive(true);

        bc.enabled = false;
    }
}
