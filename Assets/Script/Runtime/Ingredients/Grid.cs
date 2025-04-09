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
            Open();
        }
    }

    void Open()
    {
        Debug.Log(gameObject.name + " is opening!");
        isOpen = true;

        Vector3 direction = new Vector3(0, -1, 0);
        transform.Translate(direction);

        if (teleportHitbox != null)
            teleportHitbox.SetActive(true);

        gameObject.SetActive(false);
    }
}
