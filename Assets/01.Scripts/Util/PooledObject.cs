using UnityEngine;
using UnityEngine.Pool;

public class PooledObject<T> where T : Component 
{
    private IObjectPool<T> pool;
    private T prefab;

    public PooledObject(string keyName,T prefab, bool collectionCheck, int defaultCapacity, int maxSize)
    {
        if (testPoolManager.Instance.objectPools.ContainsKey(keyName))
        {
            return;
        }
        this.prefab = prefab;
        pool = new ObjectPool<T>(CreateObject, OnGetObject, OnReleaseObject, OnDestroyObject, collectionCheck, defaultCapacity, maxSize);
        testPoolManager.Instance.objectPools[keyName] = pool;
    }

    private T CreateObject()
    {
        T objectInstance = Object.Instantiate(prefab);
        if (objectInstance is ISetPooledObject<T> settableObject)
        {
            settableObject.SetPooledObject(pool);
        }
        return objectInstance;
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
