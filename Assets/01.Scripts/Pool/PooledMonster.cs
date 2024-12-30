using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PooledMonster : MonoBehaviour, ISetPooledObject<PooledMonster>
{
    [SerializeField] public MonsterManager monsterManager{get; private set;}
    protected IObjectPool<PooledMonster> objectPool;
    public IObjectPool<PooledMonster> ObjectPool 
    { get => objectPool; set => objectPool = value; }

    private void Awake()
    {
        if (monsterManager == null)
        {
            monsterManager = GetComponent<MonsterManager>();
        }
    }

    public void SetPooledObject(IObjectPool<PooledMonster> pool)
    {
        ObjectPool = pool;
    }
}
