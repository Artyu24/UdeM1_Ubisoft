using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClientIA : MonoBehaviour, ISlideable
{
    [Header("Sound")] 
    [SerializeField] private MusicTimer _musicTimer;

    [Header("Components")] 
    [SerializeField] private Animator _animController;
    
    [Header("IA")]
    [SerializeField] private NavMeshAgent _clientAgent;
    [SerializeField] private Collider _rangeColliderPush;
    [SerializeField] private List<Transform> _wanderPoints = new List<Transform>();
    private int _wanderIndex = 0;

    [Header("Water Fall")] 
    [SerializeField] private float _fallenTime = 2;

    [Header("Water Slide")] 
    private bool _isSliding;
    private Coroutine _getUpAfterSlideCoroutine;
    [SerializeField] private Transform _iaMesh;
    [SerializeField] private float _rotationSpeedAnim = 500;
    
    private void Awake()
    {
        if(_wanderPoints.Count != 0)   
            _clientAgent.destination = _wanderPoints[0].position;
    }
    
    private void Update()
    {
        if (_musicTimer.DoRandomSound)
        {
            if(AudioManager.instance != null)
                AudioManager.instance.PlayRandom(SoundState.SFX_HUMAN_VOICES);
            _musicTimer.DoRandomSound = false;
        }
        
        //Wander btw point
        if(_wanderPoints.Count == 0)
            return;
        
        if (_musicTimer.DoSound)
        {
            if(AudioManager.instance != null)
                AudioManager.instance.PlayRandom(SoundState.SFX_HUMAN_FOOTSTEPS);
            _musicTimer.DoSound = false;
        }
        
        if (!_clientAgent.pathPending)
        {
            if (_clientAgent.remainingDistance <= _clientAgent.stoppingDistance)
            {
                if (!_clientAgent.hasPath || _clientAgent.velocity.sqrMagnitude == 0f)
                {
                    //Reset Sliding Anim
                    if (_isSliding)
                    {
                        if (_getUpAfterSlideCoroutine == null)
                        {
                            _animController.SetTrigger("GetUp");
                            _getUpAfterSlideCoroutine = StartCoroutine(IAGetUpAfterSlide());
                        }
                        return;
                    }
                    
                    //Next Point
                    _wanderIndex++;
                    if (_wanderPoints.Count <= _wanderIndex)
                        _wanderIndex = 0;

                    _clientAgent.destination = _wanderPoints[_wanderIndex].position;
                }
            }
        }
    }
    
    public void OnRangeTriggerEnter(Collider other)
    {
        PlayerMovement pM = other.gameObject.GetComponent<PlayerMovement>();
        if (pM != null)
        {
            pM.OnIAPush(transform.position);
        }
    }

    public void OnSlide(bool doesContainsOil)
    {
        if(AudioManager.instance != null)
            AudioManager.instance.PlayRandom(SoundState.SFX_HUMAN_SLIP_ON_WATER);
            
        _animController.SetTrigger("Fall");
        
        if (!doesContainsOil)
        {
            //Fall
            _clientAgent.isStopped = true;
            _rangeColliderPush.enabled = false;
            
            StartCoroutine(IAFalling());
        }
        else
        {
            //Slide
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit))
            {
                _clientAgent.destination = hit.point;
                _isSliding = true;
                _wanderIndex--;
            }
        }
    }

    private IEnumerator IAFalling()
    {
        yield return new WaitForSeconds(_fallenTime - 1);
        _animController.SetTrigger("GetUp");
        yield return new WaitForSeconds(_fallenTime);
        _clientAgent.isStopped = false;
        _rangeColliderPush.enabled = true;
    }

    private IEnumerator IAGetUpAfterSlide()
    {
        yield return new WaitForSeconds(2f);
        _isSliding = false;
        _getUpAfterSlideCoroutine = null;
    }
}