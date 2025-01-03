using UnityEngine;
using UnityEngine.Pool;

public class testScript : MonoBehaviour, ISetPooledObject<testScript>
{
    public int foolCount = 5;

    private IObjectPool<testScript> objectPool;
    public IObjectPool<testScript> ObjectPool
    {
        get => objectPool;
        set => objectPool = value;
    }
    public void SetPooledObject(IObjectPool<testScript> pool)
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
