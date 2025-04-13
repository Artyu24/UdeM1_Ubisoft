using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [Scene, SerializeField] string SceneName;
    [Button]
    public void Back()
    {
        // load game scene
        SceneManager.LoadScene(SceneName);
    }
}
