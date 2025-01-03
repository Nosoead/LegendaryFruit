using UnityEngine;
using UnityEngine.Pool;

public class testScript2 : MonoBehaviour, ISetPooledObject<testScript2>
{
    private IObjectPool<testScript2> objectPool;
    public IObjectPool<testScript2> ObjectPool
    {
        get => objectPool;
        set => objectPool = value;
    }
    public void SetPooledObject(IObjectPool<testScript2> pool)
    {
        ObjectPool = pool;
    }

    public void ReleaseObject()
    {
        Invoke(nameof(DelayMethod), 3f);
    }

    private void DelayMethod()
    {
        objectPool.Release(this);
    }
}
