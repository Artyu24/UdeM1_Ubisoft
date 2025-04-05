using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Scene, SerializeField] string SceneName;
    [Button]
    public void LoadGame()
    {
        // load game scene
        SceneManager.LoadScene(SceneName); 
    }
}
