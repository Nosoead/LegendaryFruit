using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//T으로 Dictionary 자동 연동해서 사용가능 하도록.
public class PoolManager : Singleton<PoolManager>
{
    public Dictionary<string, object> objectPools = new Dictionary<string, object>();

    [SerializeField] private testScript testScriptPrefab;
    [SerializeField] private testScript2 testScript2Prefab;

    private int initNum = 5;

    private PooledObject<testScript> pool1;
    private PooledObject<testScript2> pool2;

    protected override void Awake()
    {
        base.Awake();
        pool1 = new PooledObject<testScript>("TestScriptPool", testScriptPrefab, true, initNum, 5);
        pool2 = new PooledObject<testScript2>("TestScript2Pool", testScript2Prefab, true, 1, 8);
        CreatePool();
    }

    public void test1ButtonGet()
    {
        var obj = pool1.Get();
        obj.ReleaseObject();
    }

    public void test2ButtonGet()
    {
        var obj = pool2.Get();
        obj.ReleaseObject();
    }

    public void CreatePool()
    {
        for (int i = 0; i < initNum; i++)
        {
            var obj = pool1.Get();
            obj.ReleaseObject();
        }
    }
}
