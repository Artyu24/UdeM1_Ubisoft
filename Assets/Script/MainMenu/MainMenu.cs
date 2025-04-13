using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Scene, SerializeField] string SceneName;
    [Scene, SerializeField] string SceneName2;
    [Button]
    public void LoadGame()
    {
        if(AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.UI_CLICK);
        
        // load game scene
        SceneManager.LoadScene(SceneName); 
    }

    [Button]
    public void LoadCredits()
    {
        if(AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.UI_CLICK);
        
        // load credits scene
        SceneManager.LoadScene(SceneName2);
    }
}
