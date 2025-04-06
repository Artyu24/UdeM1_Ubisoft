using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class PipeTool : MonoBehaviour
{
    [Header("Leak")]
    [SerializeField] private List<LeakPuddle> _leakPuddlesList = new List<LeakPuddle>();
    [SerializeField, ReadOnly] private List<DropWater> _leakDropWaterList = new List<DropWater>();
    private List<DropWater> _alreadyLeakDropWaterList = new List<DropWater>();
    private float _effectDuration;
    private float _timer = 100f;
    
    [Header("Components")]
    [SerializeField] private SplineContainer _splineContainer;
    [SerializeField, ReadOnly] private DropWater _dropWaterPipe;
    public DropWater DropWaterPipe => _dropWaterPipe;
    
    [Header("Prefab")]
    [SerializeField] private Transform _pipePrefab;
    [SerializeField] private Transform _pipeTurningPrefab;
    [SerializeField] private DropWater _dropWaterPrefab;
    [SerializeField] private SplineAnimate _pipeEffectPrefab;

    public SplineAnimate PipeEffect()
    {
        SplineAnimate animEffect = Instantiate(_pipeEffectPrefab);
        animEffect.Container = _splineContainer;
        animEffect.Play();

        //Leak
        _effectDuration = animEffect.Duration;
        _timer = 0;
        _alreadyLeakDropWaterList.Clear();
        
        return animEffect;
    }

    private void Update()
    {
        if(_timer > _effectDuration)
            return;
        
        _timer += Time.deltaTime;
        float percent = Mathf.Lerp(0, 1, _timer / _effectDuration);

        for (int i = 0; i < _leakDropWaterList.Count; i++)
        {
            if(_alreadyLeakDropWaterList.Contains(_leakDropWaterList[i]))
                continue;

            if (_leakPuddlesList[i].LeakPercentPath < percent)
            {
                _leakDropWaterList[i].DropWaterBelow(true);
                _alreadyLeakDropWaterList.Add(_leakDropWaterList[i]);
            }
        }
    }

    [Button]
    private void CreatePipe()
    {
        //Spline Knot Right Position
        for (int i = 1; i < _splineContainer.Spline.Knots.Count(); i++)
        {
            switch (GetMostDistAxis(_splineContainer.Spline[i - 1].Position, _splineContainer.Spline[i].Position).Item1)
            {
                case Axis.X:
                    _splineContainer.Spline.SetKnot(i, new BezierKnot(new float3(_splineContainer.Spline[i].Position.x, _splineContainer.Spline[i - 1].Position.y, _splineContainer.Spline[i - 1].Position.z)));
                    break;
                case Axis.Y:
                    _splineContainer.Spline.SetKnot(i, new BezierKnot(new float3(_splineContainer.Spline[i - 1].Position.x, _splineContainer.Spline[i].Position.y, _splineContainer.Spline[i - 1].Position.z)));
                    break;
                case Axis.Z:
                    _splineContainer.Spline.SetKnot(i, new BezierKnot(new float3(_splineContainer.Spline[i - 1].Position.x, _splineContainer.Spline[i - 1].Position.y, _splineContainer.Spline[i].Position.z)));
                    break;
            }
        }

        //Reset Old Pipe
        if (transform.childCount != 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == "PipeOffset")
                {
                    DestroyImmediate(transform.GetChild(i).gameObject);
                    break;
                }
            }
            
            _leakDropWaterList.Clear();
        }
            
        //Pipe
        GameObject parentObject = new GameObject("PipeOffset");
        parentObject.transform.parent = transform;
        parentObject.transform.SetSiblingIndex(0);
        parentObject.transform.localPosition = Vector3.zero;
        parentObject.transform.localScale = Vector3.one;
        
        for (int i = 1; i < _splineContainer.Spline.Knots.Count(); i++)
        {
            Transform pipe = Instantiate(_pipePrefab, parentObject.transform);

            (Axis, float) mostDistAxis = GetMostDistAxis(_splineContainer.Spline[i - 1].Position, _splineContainer.Spline[i].Position);
            if (mostDistAxis.Item2 - 1 > 0)
            {
                switch (mostDistAxis.Item1)
                {
                    case Axis.X:
                        pipe.eulerAngles = new Vector3(0, 0, 90);
                        break;
                    case Axis.Y:
                        pipe.eulerAngles = new Vector3(0, 0, 0);
                        break;
                    case Axis.Z:
                        pipe.eulerAngles = new Vector3(90, 0, 0);
                        break;
                }

                pipe.localPosition = Vector3.Lerp(_splineContainer.Spline[i - 1].Position, _splineContainer.Spline[i].Position, 0.5f);
                pipe.localScale = new Vector3(1, (mostDistAxis.Item2 - 1), 1);
            }
            else
            {
                DestroyImmediate(pipe.gameObject);
            }
            
            //Turning Pipe
            if (i < _splineContainer.Spline.Knots.Count() - 1)
            {
                Transform turningPipe = Instantiate(_pipeTurningPrefab, parentObject.transform);
                turningPipe.localPosition = _splineContainer.Spline[i].Position;
                
                Vector3 directionBefore = _splineContainer.Spline[i].Position - _splineContainer.Spline[i - 1].Position;
                directionBefore.Normalize();
                Vector3 directionAfter = _splineContainer.Spline[i + 1].Position - _splineContainer.Spline[i].Position;
                directionAfter.Normalize();
                Vector3 rotationAxis = Vector3.Cross(directionBefore, directionAfter).normalized;
                float angle = Vector3.Angle(directionBefore, directionAfter);
                turningPipe.rotation = Quaternion.AngleAxis(angle, rotationAxis) * Quaternion.LookRotation(directionBefore, rotationAxis) * Quaternion.Euler(0, 0, 90);
            }
            else
            {
                //Drop Water At End
                _dropWaterPipe = Instantiate(_dropWaterPrefab, parentObject.transform);
                _dropWaterPipe.transform.localPosition = _splineContainer.Spline[i].Position;
            }
        }

        //Add Leak
        for (int i = 0; i < _leakPuddlesList.Count; i++)
        {
            DropWater dropWaterLeak = Instantiate(_dropWaterPrefab, parentObject.transform);
            dropWaterLeak.transform.position = _splineContainer.EvaluatePosition(_leakPuddlesList[i].LeakPercentPath);
            dropWaterLeak.gameObject.name = "Leak_" + (i + 1);
            _leakDropWaterList.Add(dropWaterLeak);
        }
    }

    private (Axis, float) GetMostDistAxis(Vector3 lastPos, Vector3 actualPos)
    {
        float xDist = Mathf.Abs(lastPos.x - actualPos.x);
        float yDist = Mathf.Abs(lastPos.y - actualPos.y);
        float zDist = Mathf.Abs(lastPos.z - actualPos.z);

        if (xDist > yDist && xDist > zDist)
            return (Axis.X, xDist);
        else if (yDist > xDist && yDist > zDist)
            return (Axis.Y, yDist);
        else
            return (Axis.Z, zDist);
    }
    
    private enum Axis
    {
        X,
        Y,
        Z
    }
    
    [Serializable]
    private struct LeakPuddle
    {
        [SerializeField, Range(0f, 1f)] private float _leakPercentPath;
        public float LeakPercentPath => _leakPercentPath;
    }
}
