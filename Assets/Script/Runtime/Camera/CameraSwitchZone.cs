using UnityEngine;

public class CameraSwitchZone : MonoBehaviour
{
    private int playerNumberInZone;
    [SerializeField] private GameObject zoomCam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerData>())
        {
            playerNumberInZone++;
            zoomCam.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerData>())
        {
            playerNumberInZone--;
            if (playerNumberInZone == 0)
            {
                zoomCam.SetActive(false);
            }
        }
    }

}
