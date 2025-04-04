using UnityEngine;

public class Grid : MonoBehaviour
{
    public int requiredObjectCount = 1;
    private bool isOpen = false;

    private void OnEnable()
    {
        Throne.OnThroneUpdated += CheckIfShouldOpen;
    }

    private void OnDisable()
    {
        Throne.OnThroneUpdated -= CheckIfShouldOpen;
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

        gameObject.SetActive(false);
    }
}
