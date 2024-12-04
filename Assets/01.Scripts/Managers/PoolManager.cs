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
        //CreatePool();
    }

    public void test1ButtonGet()
    {
        var obj = reward.Get();
        obj.ReleaseObject();
    }

    public void test2ButtonGet()
    {
        var obj = reward.Get();
        obj.ReleaseObject();
    }

    public void CreatePool()
    {
        for (int i = 0; i < initNum; i++)
        {
            var obj = reward.Get();
            obj.ReleaseObject();
        }
    }
}
