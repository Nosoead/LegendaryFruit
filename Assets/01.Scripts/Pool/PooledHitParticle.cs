using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PooledHitParticle : MonoBehaviour, ISetPooledObject<PooledHitParticle>
{
    protected IObjectPool<PooledHitParticle> objectPool;
    public IObjectPool<PooledHitParticle> ObjectPool
    { get => objectPool; set => objectPool = value; }

    public void SetPooledObject(IObjectPool<PooledHitParticle> pool)
    {
        ObjectPool = pool;
    }
}
