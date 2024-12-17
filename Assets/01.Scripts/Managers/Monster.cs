using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

public class Monster : MonoBehaviour, ISetPooledObject<Monster>
{
    private IObjectPool<Monster> objectPool;

    public IObjectPool<Monster> ObjectPool
    {
        get => objectPool;
        set => objectPool = value;
    }

    public void SetPooledObject(IObjectPool<Monster> pool)
    {
        objectPool = pool;
    }
}