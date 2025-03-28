using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.Splines.ExtrusionShapes;

public class PipeTool : MonoBehaviour
{
    [SerializeField] private SplineContainer _splineContainer;

    [SerializeField] private SplineAnimate _pipeEffectPrefab;
    
    public void PipeEffect()
    {
        SplineAnimate animEffect = Instantiate(_pipeEffectPrefab);
        animEffect.Container = _splineContainer;
        animEffect.Play();
        animEffect.Completed += () => { Destroy(animEffect.gameObject); };
    }
    
    [SerializeField] private Transform _pipePrefab;
    [SerializeField] private Transform _pipeTurningPrefab;
    
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
        
        if(transform.childCount != 0)
            DestroyImmediate(transform.GetChild(0).gameObject);
            
        //Pipe
        GameObject parentObject = new GameObject("PipeOffset");
        parentObject.transform.parent = transform;
        parentObject.transform.localPosition = Vector3.zero;

        for (int i = 1; i < _splineContainer.Spline.Knots.Count(); i++)
        {
            Transform pipe = Instantiate(_pipePrefab, parentObject.transform);

            (Axis, float) mostDistAxis = GetMostDistAxis(_splineContainer.Spline[i - 1].Position, _splineContainer.Spline[i].Position);
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
            pipe.localScale = new Vector3(1, mostDistAxis.Item2 / 2 * 1.9f, 1);

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
}
