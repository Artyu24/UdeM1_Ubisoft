using TMPro;
using UnityEngine;

public class LoadLevelButton : MonoBehaviour
{
    [SerializeField] GameObject Button1;
    [SerializeField] GameObject Button2;   
    [SerializeField] GameObject Button3;

    [SerializeField] TextMeshProUGUI toggleButtonText;

    private bool levelsVisible = false;

    public void ToggleLevels()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.UI_CLICK);

        levelsVisible = !levelsVisible;
        Button1.SetActive(levelsVisible);
        Button2.SetActive(levelsVisible);
        Button3.SetActive(levelsVisible);

        toggleButtonText.text = levelsVisible ? "Hide Levels" : "Load Levels";
    }
}
