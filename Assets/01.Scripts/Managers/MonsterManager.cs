using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    private MonsterSO monsterData;

    //Pattren
    private Dictionary<int, PatternData> pattrens = new Dictionary<int, PatternData>();
    [SerializeField] private MonsterStatManager monsterStatManager;
    [SerializeField] private MonsterAnimationController monsterAnimationController;

    private void Awake()
    {
        
    }
    private void Start()
    {
        //GetMonsterData(101);
    }

    public void GetMonsterData(int id)
    {
        monsterData = EntityManager.Instance.GetMonster(id);
        SetMonsterData();
    }

    private void SetMonsterData()
    {
        monsterStatManager.SetInitStat(monsterData);
        monsterAnimationController.SetInitMonsterAnimation(monsterData);
        if(monsterData is BossMonsterSO bossMonsterData)
        {
            var data = bossMonsterData.pattrens;
            for(int i = 0; i < data.Count; i++)
            {
                pattrens.Add(data[i].pattrenID, data[i]);
            }
            monsterStatManager.SetPattrenStat(pattrens);
            monsterAnimationController.SetPatternAnimation(pattrens);
        }
    }
}