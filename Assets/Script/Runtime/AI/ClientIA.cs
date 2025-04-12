using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClientIA : MonoBehaviour, ISlideable
{
    [Header("Sound")] 
    [SerializeField] private MusicTimer _musicTimer;
    
    [Header("IA")]
    [SerializeField] private NavMeshAgent _clientAgent;
    [SerializeField] private Collider _rangeColliderPush;
    [SerializeField] private List<Transform> _wanderPoints = new List<Transform>();
    private int _wanderIndex = 0;

    [Header("Water Fall")] 
    [SerializeField] private float _fallenTime = 2;

    [Header("Water Slide")] 
    private bool _isSliding;
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
        
        //Anim when Sliding
        if(_isSliding)
            _iaMesh.Rotate(transform.up * _rotationSpeedAnim * Time.deltaTime);
        
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
                        _isSliding = false;
                        _iaMesh.eulerAngles = Vector3.zero;
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
        yield return new WaitForSeconds(_fallenTime);
        _clientAgent.isStopped = false;
        _rangeColliderPush.enabled = true;
    }
}