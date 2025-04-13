using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Scene, SerializeField] string SceneName;
    [Scene, SerializeField] string Credits;

    [Scene, SerializeField] string Level1;
    [Scene, SerializeField] string Level2;
    [Scene, SerializeField] string Level3;

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
        SceneManager.LoadScene(Credits);
    }

    [Button]
    public void LoadLevel1()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.UI_CLICK);

        SceneManager.LoadScene(Level1);
    }

    [Button]
    public void LoadLevel2()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.UI_CLICK);

        SceneManager.LoadScene(Level2);
    }

    [Button]
    public void LoadLevel3()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.UI_CLICK);

        SceneManager.LoadScene(Level3);
    }
}
