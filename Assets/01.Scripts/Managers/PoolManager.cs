using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
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
        //reward = new PooledObject<Reward>("Reward", rewardPrefab, true, initNum, 5);
        pool2 = new PooledObject<testScript2>("TestScript2Pool", testScript2Prefab, true, 1, 8);
    }

    //public void test2ButtonGet()
    //{
    //    var obj = reward.Get();
    //    obj.ReleaseObject();
    //}


    // 오브젝트 풀를 만드는 함수
    public void CreatePool<T>(T obj) where T : Component, ISetPooledObject<T>
    {
        var pool = new PooledObject<T>($"{nameof(obj)}", obj, false, 5, 5);
        objectPools.Add(typeof(T).Name, pool);
    }

    // 풀 자체를 꺼내는 함수
    public object GetPool<T>()
    {
        objectPools.TryGetValue(typeof(T).Name, out var pool);
        return pool;
    }

    // 오브젝트를 꺼내는 함수
    public object GetObject<T>()
    {
        PooledObject<T> obj =(PooledObject<T>)GetPool<T>();
        return  obj;
    }
}
