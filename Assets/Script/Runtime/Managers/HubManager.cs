using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class HubManager : MonoBehaviour
{
    [SerializeField] private List<SceneRespawnPoint> _sceneRespawnPoints = new List<SceneRespawnPoint>();

    [Button]
    private void NewSpawnPoint()
    {
        GameObject g = new GameObject("SpawnPoint_" + (_sceneRespawnPoints.Count + 1));
        g.transform.parent = transform;
        _sceneRespawnPoints.Add(new SceneRespawnPoint(g.transform));
    }

    public Transform TeleportPlayer(string sceneName)
    {
        foreach (SceneRespawnPoint respawnPoint in _sceneRespawnPoints)
        {
            if (respawnPoint.SceneName == sceneName)
            {
                return respawnPoint.RespawnPoint;
            }
        }

        return null;
    }
    
    [Serializable]
    private struct SceneRespawnPoint
    {
        [SerializeField, Scene] private string _sceneName;
        [SerializeField] private Transform _respawnPoint;
        
        public string SceneName => _sceneName;
        public Transform RespawnPoint => _respawnPoint;
        public SceneRespawnPoint(Transform respawnPoint)
        {
            _sceneName = default;
            _respawnPoint = respawnPoint;
        }

    }
}
