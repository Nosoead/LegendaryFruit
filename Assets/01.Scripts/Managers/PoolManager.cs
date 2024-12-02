using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : Singleton<PoolManager>
{
    public Dictionary<string, object> objectPools = new Dictionary<string, object>();

    [SerializeField] private testScript testScriptPrefab;
    [SerializeField] private testScript2 testScript2Prefab;

    private PooledObject<testScript> pool1;
    private PooledObject<testScript2> pool2;

    protected override void Awake()
    {
        base.Awake();
        pool1 = new PooledObject<testScript>("TestScriptPool", testScriptPrefab, true, 1, 5);
        pool2 = new PooledObject<testScript2>("TestScript2Pool", testScript2Prefab, true, 1, 8);
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
}
