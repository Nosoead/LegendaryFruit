using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface ISetPooledObject<T> where T : Component
{
    void SetPooledObject(IObjectPool<T> pool);
}
public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<PoolType, GameObject> prefabDictionary = new Dictionary<PoolType, GameObject>();
    private Dictionary<PoolType, object> poolDictionary = new Dictionary<PoolType, object>();
    private Dictionary<PoolType, Action> resetDictionary = new Dictionary<PoolType, Action>();

    protected override void Awake()
    {
        base.Awake();
        RegisterPrefab();
    }

    public void Init()
    {
        RegisterPrefab();
    }

    private void RegisterPrefab()
    {
        for (int i = 0; i < (int)PoolType.Count; i++)
        {          
            PoolType poolType = (PoolType)i;
            GameObject poolObject = ResourceManager.Instance.LoadResource<GameObject>($"Pool/{poolType}");
            if (!prefabDictionary.ContainsKey((PoolType)i) && poolObject != null)
            {
                prefabDictionary.Add((PoolType)i, poolObject);
            }
        }
        // TODO : 모든 풀링오브젝트를 미리 CreatePool
        PreCreateObjectPool();
    }

    public void CreatePool<T>(PoolType poolType, bool collectionCheck, int defaultCapacity, int maxSize) where T : Component
    {
        if (poolDictionary.ContainsKey(poolType)) return;
        prefabDictionary.TryGetValue(poolType, out GameObject gameObjectPrefab);
        if (gameObjectPrefab.TryGetComponent<T>(out T prefab))
        {
            new GenericPooledObject<T>(poolType, prefab, collectionCheck, defaultCapacity, maxSize);
        }
    }

    private class GenericPooledObject<T> where T : Component
    {
        private IObjectPool<T> objectPoolT;
        private T gameObjectPrefab;

        public GenericPooledObject(PoolType poolType, T prefab, bool collectionCheck, int defaultCapacity, int maxSize)
        {
            if (PoolManager.Instance.poolDictionary.ContainsKey(poolType))
            {
                return;
            }
            this.gameObjectPrefab = prefab;
            objectPoolT = new ObjectPool<T>(CreateObject, OnGetObject, OnReleaseObject, OnDestroyObject, collectionCheck, defaultCapacity, maxSize);
            if (!PoolManager.Instance.poolDictionary.ContainsKey(poolType))
            {
                PoolManager.Instance.poolDictionary.Add(poolType, objectPoolT);
                PoolManager.Instance.resetDictionary.Add(poolType, objectPoolT.Clear);
            }
        }

        private T CreateObject()
        {
            T objectInstance = Instantiate(gameObjectPrefab);
            if (objectInstance is ISetPooledObject<T> settableObject)
            {
                settableObject.SetPooledObject(objectPoolT);
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
            Destroy(obj.gameObject);
        }
    }

    public IObjectPool<T> GetObjectFromPool<T>(PoolType poolType) where T : Component
    {
        if (poolDictionary.TryGetValue(poolType, out var pool) && pool is IObjectPool<T> poolComponent)
        {
            return poolComponent;
        }
        return null;
    }

    public void ResetObjectPool<T>(PoolType poolType) where T : Component
    {
        if (poolDictionary.TryGetValue(poolType, out var pool) && pool is IObjectPool<T> poolComponent)
        {
            poolComponent.Clear();
        }
    }

    public void ResetAllObjectPool()
    {
        foreach (var pool in resetDictionary.Values)
        {
            pool();
        }
    }

    private void PreCreateObjectPool()
    {
        Instance.CreatePool<PooledMonster>(PoolType.PooledMonster, false, 7, 12);
        Instance.CreatePool<PooledBossMonster>(PoolType.PooledBossMonster, false, 7, 12);
        Instance.CreatePool<PooledParticle>(PoolType.PooledParticle, false, 5, 10);
        Instance.CreatePool<PooledProjectile>(PoolType.PooledProjectile, false, 15, 30);
        Instance.CreatePool<PooledFruitWeapon>(PoolType.PooledFruitWeapon, false, 5, 5);
        Instance.CreatePool<PooledReward>(PoolType.PooledReward, false, 5, 5);
        Instance.CreatePool<PooledCurrency>(PoolType.PooledCurrency, false, 5, 5);
        Instance.CreatePool<PooledPotion>(PoolType.PooledPotion, false, 5, 5);
    }
};