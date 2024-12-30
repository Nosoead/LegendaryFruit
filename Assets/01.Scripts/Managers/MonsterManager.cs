using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private MonsterSO monsterData;
    [SerializeField] private MonsterStatManager monsterStatManager;
    [SerializeField] private MonsterAnimationController monsterAnimationController;
    [SerializeField] private BossMonsterController bossMonsterController;

    private void Awake()
    {
        //임의적으로 보스
        GetMonsterData(101);
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
            bossMonsterController.PatternData(bossMonsterData);
        }
    }

}