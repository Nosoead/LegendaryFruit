using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PooledBossMonster : MonoBehaviour, ISetPooledObject<PooledBossMonster>
{
    public MonsterManager monsterManager { get; private set; }
    protected IObjectPool<PooledBossMonster> objectPool;
    public IObjectPool<PooledBossMonster> ObjectPool
    { get => objectPool; set => objectPool = value; }

    private void Awake()
    {
        if (monsterManager == null)
        {
            monsterManager = GetComponentInChildren<MonsterManager>();
        }
    }

    public void SetPooledObject(IObjectPool<PooledBossMonster> pool)
    {
        ObjectPool = pool;
    }
}
