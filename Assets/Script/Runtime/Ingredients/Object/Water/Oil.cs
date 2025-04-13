using System;
using DG.Tweening;
using UnityEngine;

public class Oil : GrabObject
{
    public Action OnOilMelts;
    
    public void MeltsOil(Action WaterPuddleChangeAction)
    {
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            WaterPuddleChangeAction.Invoke();
            if (OnOilMelts != null)
                OnOilMelts.Invoke();
        });
    }
}
