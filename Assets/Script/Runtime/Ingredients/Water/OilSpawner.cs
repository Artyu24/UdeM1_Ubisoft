using System;
using DG.Tweening;
using UnityEngine;

public class OilSpawner : MonoBehaviour
{
    [SerializeField] private Oil _oil;

    private void Awake()
    {
        _oil.OnOilMelts += ResetOil;
    }

    private void ResetOil()
    {
        _oil.transform.position = transform.position;
        _oil.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);
    }
}
