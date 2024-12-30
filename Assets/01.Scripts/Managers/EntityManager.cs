using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : Singleton<EntityManager>
{
    public MonsterSO monsterData;
    private Dictionary<int, MonsterSO> monsters = new Dictionary<int, MonsterSO>();

    protected override void Awake()
    {
        base.Awake();
        LoadAllMonsters();
    }


    //임시
    public void LoadAllMonsters()
    {
        var monsterData = ResourceManager.Instance.LoadAllResources<MonsterSO>("MonsterSO");
        foreach (var data in monsterData)
        {
            monsters.Add(data.monsterID, data);
        }
    }

    public MonsterSO GetMonster(int id)
    {
        if (monsters.TryGetValue(id, out MonsterSO monsterData))
        {
            monsterData = monsters[id];
            return monsterData;
        }
        return null;
    }
}
