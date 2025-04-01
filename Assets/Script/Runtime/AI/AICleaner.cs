using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CleanerState
{
    cleaning,
    WalkToNewWater,
    WalkToFirstPuddle,
    Leaving,
    Balising, //can take few secondes
}

public class AICleaner : AIBehavior//il netois toujours la premiere de la liste
{
    [SerializeField] CleanerState _cleanerState;
    [SerializeField] GameObject _balise;

    [field: SerializeField] public List<WaterPuddle> ToClean { get; set; } = new List<WaterPuddle>();

    private Coroutine _DropBaliseCorou;

    private void Start()
    {
        GoToFirstPuddle();
    }
    public void AddWater(WaterPuddle waterToClean)
    {
        ToClean.Add(waterToClean);
        AiBrain.SetDestination(waterToClean.transform.position);
    }
    //Got to wate,  State: WalkToWater
    public override void ReachAIDestination()
    {
        switch(_cleanerState)
        {
            case CleanerState.WalkToNewWater:
                DropBalise(ToClean[ToClean.Count - 1].transform);
                break;
            case CleanerState.WalkToFirstPuddle:
                Clean();
                break;
        }
    }
    [Button]
    public void GoToNewWater()
    {
        AiBrain.SetDestination(ToClean[ToClean.Count-1].transform.position);
        _cleanerState = CleanerState.WalkToNewWater;
    }
    public void Clean()
    {
        _cleanerState = CleanerState.cleaning;
        StartCoroutine(CleanCoroutine());
    }
    public void GoToFirstPuddle()
    {
        AiBrain.SetDestination(ToClean[0].transform.position);
        _cleanerState = CleanerState.WalkToFirstPuddle;
    }
    public void DropBalise(Transform position)
    {
        Instantiate(_balise,position.position,Quaternion.identity);
        _cleanerState = CleanerState.Balising;
        if(_DropBaliseCorou == null)
            _DropBaliseCorou = StartCoroutine(DropBaliseAction());
    }
    private IEnumerator DropBaliseAction() 
    {
        yield return new WaitForSeconds(1);
        _DropBaliseCorou = null;
        GoToFirstPuddle();
    }
    private IEnumerator CleanCoroutine()
    {
        while(_cleanerState == CleanerState.cleaning)
        {
            yield return new WaitForSeconds(1f);
            if (ToClean[0].CleanPuddle() < 0)
            {
                ToClean.RemoveAt(0);
                break;
            }
        }
    }
    private void GoToNextPuddle()
    {
        _cleanerState = CleanerState.WalkToFirstPuddle;
    }
}
