using DG.Tweening;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int requiredObjectCount = 1;
    private bool isOpen = false;
    public GameObject teleportHitbox;

    private void OnEnable()
    {
        Throne.OnThroneUpdated += CheckIfShouldOpen;
    }

    private void OnDisable()
    {
        Throne.OnThroneUpdated -= CheckIfShouldOpen;
    }

    private void Start()
    {
        if (teleportHitbox != null) 
            teleportHitbox.SetActive(false);
    }

    void CheckIfShouldOpen(int currentCount)
    {
        if (!isOpen && currentCount >= requiredObjectCount)
        {
            Open(currentCount);
        }
    }

    void Open(int currentCount)
    {
        Debug.Log(gameObject.name + " is opening!");
        isOpen = true;

        transform.DOMove(transform.position + Vector3.down, 2f).SetEase(Ease.InOutCirc).Loops();

        if (teleportHitbox != null && currentCount == requiredObjectCount)
            teleportHitbox.SetActive(true);

        //gameObject.SetActive(false);
    }
}
