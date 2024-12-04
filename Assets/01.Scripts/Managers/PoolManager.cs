using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//T으로 Dictionary 자동 연동해서 사용가능 하도록.
public class PoolManager : Singleton<PoolManager>
{
    public Dictionary<string, object> objectPools = new Dictionary<string, object>();

    [SerializeField] private Reward rewardPrefab;
    [SerializeField] private testScript2 testScript2Prefab;

    private int initNum = 5;

    private PooledObject<Reward> reward;
    private PooledObject<testScript2> pool2;

    protected override void Awake()
    {
        base.Awake();
        reward = new PooledObject<Reward>("Reward", rewardPrefab, true, initNum, 5);
        pool2 = new PooledObject<testScript2>("TestScript2Pool", testScript2Prefab, true, 1, 8);
        CreatePool();
    }

    // return을 Reward로 받아서 RewardTree에서 위치 조정
    public Reward CreatReward()
    {
        var obj = reward.Get();
        //obj.ReleaseObject();
        return obj;
    }

    public void test2ButtonGet()
    {
        var obj = reward.Get();
        obj.ReleaseObject();
    }

    // 일단 어케할지 몰라서 적음
    // 일단 딕셔너리를 만들어 키값을 int로 받아 만들때 자동으로 키 벨류 자동 저장

    private int rewardKey = 1;
    public Dictionary<int, Reward> rewards = new Dictionary<int, Reward>();

    public void CreatePool()
    {
        for (int i = 0; i < initNum; i++)
        {
            var obj = reward.Get();
            obj.ReleaseObject();
            rewards.Add(rewardKey++,obj);
        }
    }
}
