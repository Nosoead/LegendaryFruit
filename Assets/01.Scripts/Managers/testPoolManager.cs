using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

// TODO: Dictionary 자동 생성하여 편리하게 사용할 수 있도록 구현
public class testPoolManager: Singleton<testPoolManager>
{
    public Dictionary<string, object> objectPools = new Dictionary<string, object>();

    protected override void Awake()
    {
        base.Awake();
    }

    // 오브젝트 풀를 만드는 함수
    public void CreatePool<T>(T obj, bool collectionCheck, int defualtCapacity, int maxSize) where T : Component, ISetPooledObject<T>
    {
        var pool = new PooledObject<T>($"{nameof(obj)}", obj, collectionCheck, defualtCapacity, maxSize);
        if(!objectPools.ContainsKey(typeof(T).Name))
        {
            objectPools.Add(typeof(T).Name, pool);
        }
    }

    // 풀 자체를 꺼내는 함수
    public object GetPool<T>() where T : Component
    {
        objectPools.TryGetValue(typeof(T).Name, out var pool);
        return pool;
    }

    // 오브젝트를 꺼내는 함수
    public T GetObject<T>() where T : Component
    {
        PooledObject<T> obj = GetPool<T>() as PooledObject<T>;
        return  obj.Get();
    }
}
