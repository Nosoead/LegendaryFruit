using UnityEngine;
using UnityEngine.Pool;

public class PooledEffect : MonoBehaviour, ISetPooledObject<PooledEffect>
{
    protected IObjectPool<PooledEffect> objectPool;
    public IObjectPool<PooledEffect> ObjectPool { set => objectPool = value; }

    public void SetPooledObject(IObjectPool<PooledEffect> pool)
    {
        ObjectPool = pool;
    }
}
