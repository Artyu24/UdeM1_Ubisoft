using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        // load game scene
        SceneManager.LoadScene(2); //load lvl 1
    }
}
