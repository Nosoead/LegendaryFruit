using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

public class StageBase :MonoBehaviour
{
    //[SerializeField] private Transform spawnPointRot;
    //private Dictionary<string, Vector2> keyValuePairs = new Dictionary<string, Vector2>();
    [SerializeField] public StageSO stageSO;
    [SerializeField] public Monster monster;
    [SerializeField] public RewardTree rewardTree;

    [SerializeField] public Reward reward;

    [SerializeField] public Transform playerSpawnPoint;
    [SerializeField] public Transform monsterSpawnPoint;
    [SerializeField] public Transform rewardSpawnPoint;


    protected virtual void Awake()
    { 
        
    }





    //public Vector2 PlayerSpawnPoint()
    //{
    //    keyValuePairs.TryGetValue("PlayerSpawnPoint", out var point);
    //    return point;
    //}

    //public Vector2 MonsterSpawnPoint()
    //{
    //    keyValuePairs.TryGetValue("MonsterSpawnPoint", out var point);
    //    return point;
    //}

    //public Vector2 RewardTreeSpawnPoint()
    //{
    //    keyValuePairs.TryGetValue("RewardTreeSpawnPoint", out var point);
    //    return point;
    //}
}
