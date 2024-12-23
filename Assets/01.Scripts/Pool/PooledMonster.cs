using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PooledMonster : MonoBehaviour, ISetPooledObject<PooledMonster>
{
    [SerializeField] public MonsterStatManager statManager{get; private set;}
    protected IObjectPool<PooledMonster> objectPool;
    public IObjectPool<PooledMonster> ObjectPool 
    { get => objectPool; set => objectPool = value; }

    private void Awake()
    {
        if (statManager == null)
        {
            statManager = GetComponent<MonsterStatManager>();
        }
    }

    public void SetPooledObject(IObjectPool<PooledMonster> pool)
    {
        ObjectPool = pool;
    }
}
