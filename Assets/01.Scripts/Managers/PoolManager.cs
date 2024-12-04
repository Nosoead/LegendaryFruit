using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//T���� Dictionary �ڵ� �����ؼ� ��밡�� �ϵ���.
public class PoolManager : Singleton<PoolManager>
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
        //CreatePool();
    }

    public void test2ButtonGet()
    {
        var obj = reward.Get();
        obj.ReleaseObject();
    }

    // �ϴ� �������� ���� ����
    // �ϴ� ��ųʸ��� ����� Ű���� int�� �޾� ���鶧 �ڵ����� Ű ���� �ڵ� ����

    private int rewardKey = 1;
    public Dictionary<int, Reward> rewards = new Dictionary<int, Reward>();

    // TODO : ���߿� ���׸����� �����丵
    public void CreatePool()
    {
        for (int i = 0; i < rewardTree.spawnPositions.Count; i++)
        {
            Vector2 rewardSpawnPonint = rewardTree.spawnPositions[i].transform.position;
            var obj = reward.Get();
            obj.SetPosition(rewardSpawnPonint);
            obj.ReleaseObject();
        }
    }
}
