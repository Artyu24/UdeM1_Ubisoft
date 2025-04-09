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
    [field: SerializeField] public List<WaterPuddle> ToBalise { get; set; } = new List<WaterPuddle>();

    private Coroutine _DropBaliseCorou;
    private Vector3 _startPoint;
    private void Start()
    {
        _startPoint = transform.position;
        GoToNewWater(null);
    }
    public void AddWater(WaterPuddle waterToAdd)
    {   
        ToBalise.Add(waterToAdd);
        ToClean.Add(waterToAdd);
        GoToNewWater(null);
         //AiBrain.SetDestination(waterToClean.transform.position);
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
    public void GoToNewWater(WaterPuddle oldWater)
    {
        if (ToBalise.Contains(oldWater))
        {
            ToBalise.Remove(oldWater);
        }
        if (ToBalise.Count > 0)
        {
            AiBrain.SetDestination(ToBalise[0].transform.position);
            _cleanerState = CleanerState.WalkToNewWater;
        }
    }
    public void Clean()
    {
        _cleanerState = CleanerState.cleaning;
        StartCoroutine(CleanCoroutine());
    }
    public void GoToFirstPuddle()
    {
        if (ToClean.Count > 0)
        {
            AiBrain.SetDestination(ToClean[0].transform.position);
            _cleanerState = CleanerState.WalkToFirstPuddle;
        }
        else
            GoToStart();
    }
    public void DropBalise(Transform position)
    {
        if (ToBalise.Count <= 0)
        {
            return;
        }
        GameObject newbalise= Instantiate(_balise, ToBalise[0].transform.position,Quaternion.identity);
        newbalise.transform.SetParent(ToBalise[0].transform);
        ToBalise[0].Sign = newbalise;
        _cleanerState = CleanerState.Balising;
        if(_DropBaliseCorou == null)
            _DropBaliseCorou = StartCoroutine(DropBaliseAction());
    }
    private IEnumerator DropBaliseAction() 
    {

        yield return new WaitForSeconds(1);
        _DropBaliseCorou = null;
        ToBalise.RemoveAt(0);
        if (ToBalise.Count > 0)
        {
            AiBrain.SetDestination(ToBalise[0].transform.position);
            _cleanerState = CleanerState.WalkToNewWater;
            GoToNewWater(ToBalise[0]);
        }
        else
            GoToFirstPuddle();
    }
    private IEnumerator CleanCoroutine()
    {
        while(_cleanerState == CleanerState.cleaning)
        {
            yield return new WaitForSeconds(1f);
            if(ToClean.Count > 0)
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
    public void OncompleteCleaning(WaterPuddle wp)
    {
        if (ToClean.Contains(wp))
        {
            ToClean.Remove(wp);
            GoToFirstPuddle();
        }
    }
    private void GoToStart()
    {
        AiBrain.SetDestination(_startPoint);
    }
}
