using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PooledMonster : MonoBehaviour, ISetPooledObject<PooledMonster>
{
    protected IObjectPool<PooledMonster> objectPool;
    public IObjectPool<PooledMonster> ObjectPool { set => objectPool = value; }

    public void InstanceRelease()
    {
        objectPool.Release(this);
    }
    public void Release3sec()
    {
        Invoke(nameof(Release), 3f);
    }
    private void Release()
    {
        objectPool.Release(this);
    }

    public void SetPooledObject(IObjectPool<PooledMonster> pool)
    {
        ObjectPool = pool;
    }
}
