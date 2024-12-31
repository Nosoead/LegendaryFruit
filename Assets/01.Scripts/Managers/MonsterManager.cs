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
        Debug.Log(monsterData.ToString());
        if(monsterData is BossMonsterSO bossMonsterData)
        {
            Debug.Log(bossMonsterData);
            bossMonsterController.PatternData(bossMonsterData);
        }
    }

}