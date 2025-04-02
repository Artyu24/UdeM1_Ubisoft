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
        gameObject.SetActive(false);
        isOpen = true;
    }
}
