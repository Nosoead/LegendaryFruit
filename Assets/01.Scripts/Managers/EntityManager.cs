using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : Singleton<EntityManager>
{
    public MonsterSO monsterData;
    private Dictionary<int,MonsterSO> monsterDatas = new Dictionary<int,MonsterSO>();

    protected override void Awake()
    {
        base.Awake();
        SettingData();
        SettingMonster(3);
    }

    private void SettingData()
    {
        var monsterData = ResourceManager.Instance.LoadAllResources<MonsterSO>("MonsterSO");
        foreach (var data in monsterData)
        {
            monsterDatas.Add(data.monsterID, data);
        }      
    }


    //임의적으로 뽑아서 일단 진행 
    public void SettingMonster(int monsterID)
    {
        if(!monsterDatas.ContainsKey(monsterID))
        {
            SettingData();
        }
        else
        {
            var value = monsterDatas[monsterID];
            monsterData = value;
        }
    }

}
