using UnityEngine;
using UnityEngine.Pool;


public class PooledObject<T> where T : Component
{
    private IObjectPool<T> pool;
    private T prefab;

    public PooledObject(string keyName,T prefab, bool collectionCheck, int defaultCapacity, int maxSize)
    {
        if (PoolManager.Instance.objectPools.ContainsKey(keyName))
        {
            return;
        }
        this.prefab = prefab;
        var pool = new ObjectPool<T>(CreateProjectile, OnGetObject, OnReleaseObject, OnDestroyObject, collectionCheck, defaultCapacity, maxSize);
        PoolManager.Instance.objectPools[keyName] = pool;
    }

    private T CreateProjectile()
    {
        T projectileInstance = Object.Instantiate(prefab);
        return projectileInstance;
    }

    private void OnGetObject(T obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnReleaseObject(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void OnDestroyObject(T obj)
    {
        Object.Destroy(obj.gameObject);
    }

    public T Get()
    {
        return pool.Get();
    }

    public void Release(T obj)
    {
        pool.Release(obj);
    }
}
