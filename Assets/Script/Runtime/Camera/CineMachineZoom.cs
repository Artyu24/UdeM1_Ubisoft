using DG.Tweening;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;



public class CineMachineZoom : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    bool hasCamStarted = false;

    [SerializeField] GameObject dollycamObject;

    CinemachineCamera cam;

    private void Start()
    {
        cam = dollycamObject.GetComponent<CinemachineCamera>();
    }

    private void PlayerHitsCamera()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Player" && !hasCamStarted)
        {

            hasCamStarted = true;
            print("start cam sequence");

            dollycamObject.gameObject.SetActive(true);

            Invoke("DisableCam", 3);

               
        }
      
    }
    void DisableCam()
    {
        dollycamObject.gameObject.SetActive(false);
    }
  
}
