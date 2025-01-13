using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSO", menuName = "ScriptableObject/StageSO", order = 3)]
public class StageSO : ScriptableObject
{
    //   AB0CD -> A : 보상종류 B : 스테이지 C : 단계 D : 버전
    //ex)13012 -> 무기보상 3-1스테이지 2버전 / 현재는 그냥 1스테이지에 전부 1버전으로
    public int stageID;
    public bool isCombatStage;
    public bool isBossStage;
    public bool canReceiveReward;
    public List<MonsterSpawnInfo> monstersToSummon = new List<MonsterSpawnInfo>();

    public int TotalMonsterCount()
    {
        if (monstersToSummon.Count == 0) return 0;
        int result = 0;
        foreach (var monsterNum in monstersToSummon)
        {
            result += monsterNum.monsterCount;
        }
        return result;
    }
    //TODO 스테이지에 따른 보상확률 조절하는 필드추가~
}

[System.Serializable]
public class MonsterSpawnInfo
{
    public int monsterCount;
    public MonsterSO monsterData;
}