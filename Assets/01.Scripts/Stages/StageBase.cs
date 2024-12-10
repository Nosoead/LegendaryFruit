using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class StageBase :MonoBehaviour
{
    [SerializeField] private Transform spawnPointRot;
    private Dictionary<string, Vector2> keyValuePairs = new Dictionary<string, Vector2>();

    public string nextStageKey;

    private void Awake()
    {
        for (int i = 0; i < spawnPointRot.childCount; i++)
        {
            keyValuePairs.Add(spawnPointRot.GetChild(i).name
                ,spawnPointRot.GetChild(i).position);
        }
    }

    public Vector2 PlayerSpawnPoint()
    {
        keyValuePairs.TryGetValue("PlayerSpawnPoint", out var point);
        return point;
    }

    public Vector2 MonsterSpawnPoint()
    {
        keyValuePairs.TryGetValue("MonsterSpawnPoint", out var point);
        return point;
    }
}
