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
        // load game scene
        SceneManager.LoadScene(SceneName); 
    }

    [Button]
    public void LoadCredits()
    {
        // load credits scene
        SceneManager.LoadScene(SceneName2);
    }
}
