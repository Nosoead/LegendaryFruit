using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PooledSound : MonoBehaviour, ISetPooledObject<PooledSound>
{
    protected IObjectPool<PooledSound> objectPool;
    public IObjectPool<PooledSound> ObjectPool { set => objectPool = value; }

    public void SetPooledObject(IObjectPool<PooledSound> pool)
    {
        ObjectPool = pool;
    }
}
