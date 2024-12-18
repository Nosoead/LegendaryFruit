using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PooledReward : MonoBehaviour, ISetPooledObject<PooledReward>
{
    protected IObjectPool<PooledReward> objectPool;
    public IObjectPool<PooledReward> ObjectPool { set => objectPool = value; }

    public void Release3sec()
    {
        Invoke(nameof(Release), 3f);
    }

    private void Release()
    {
        objectPool.Release(this);
    }

    public void SetPooledObject(IObjectPool<PooledReward> pool)
    {
        ObjectPool = pool;
    }
}
