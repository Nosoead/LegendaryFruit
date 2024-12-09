using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

// TODO: Dictionary 자동 생성하여 편리하게 사용할 수 있도록 구현
public class PoolManager: Singleton<PoolManager>
{
    public Dictionary<string, object> objectPools = new Dictionary<string, object>();

    [SerializeField] private Reward rewardPrefab;
    [SerializeField] private testScript2 testScript2Prefab;
    [SerializeField] private RewardTree rewardTree;

    private int initNum = 5;

    private PooledObject<Reward> reward;
    private PooledObject<testScript2> pool2;

    protected override void Awake()
    {
        base.Awake();
        reward = new PooledObject<Reward>("Reward", rewardPrefab, true, initNum, 5);
        pool2 = new PooledObject<testScript2>("TestScript2Pool", testScript2Prefab, true, 1, 8);
    }

    public void test2ButtonGet()
    {
        var obj = reward.Get();
        obj.ReleaseObject();
    }

    // 리스트 생성 후 -> 선택한 Reward를 리스트에 추가
    public List<Reward> rewards = new List<Reward>();

    // TODO: 이후에 그래픽적으로 개선할 것
    public void CreatePool<T>() where T : Component, ISetPooledObject<T>
    {
        for (int i = 0; i < rewardTree.spawnPositions.Count; i++)
        {
            Vector2 rewardSpawnPonint = rewardTree.spawnPositions[i].transform.position;
            var obj = reward.Get();
            obj.SetPosition(rewardSpawnPonint);
            obj.gameObject.SetActive(false);
            rewards.Add(obj);
        }
    }
}
