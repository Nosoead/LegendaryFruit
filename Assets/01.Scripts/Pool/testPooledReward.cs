using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class testPooledReward : MonoBehaviour, ISetPooledObject<testPooledReward>
{
    protected IObjectPool<testPooledReward> objectPool;
    public IObjectPool<testPooledReward> ObjectPool { set => objectPool = value; }

    public void Release3sec()
    {
        Invoke(nameof(Release), 3f);
    }

    private void Release()
    {
        objectPool.Release(this);
    }

    public void SetPooledObject(IObjectPool<testPooledReward> pool)
    {
        ObjectPool = pool;
    }
}
