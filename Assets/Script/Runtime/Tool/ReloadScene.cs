using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerManager.instance.ReloadPlayer();
            StartCoroutine(Delay());
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.05f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
