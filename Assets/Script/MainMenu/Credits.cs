using UnityEngine;
using NaughtyAttributes;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    [Scene, SerializeField] string SceneName;
    [Button]
    public void Back()
    {
        if (AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.UI_CLICK);

        // load game scene
        SceneManager.LoadScene(SceneName);
    }
}
